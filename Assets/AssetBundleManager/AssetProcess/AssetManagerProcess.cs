using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AssetBundles {

    public abstract class AssetManagerProcess {

        public string[] ActiveVariants = { };

        protected string[] _bundlesWithVariant = null;   //初始化時紀錄擁有的bundle

        //初始化
        public abstract IEnumerator Initialize(string[] uris);

        public abstract AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode);

        public abstract AssetBundleLoadAssetBaseOperation LoadAssetAsync<T>(string bundle, string asset) where T : UnityEngine.Object;

        public abstract void Dispose();

        public void AddVariant(string variant) {
            if (!ActiveVariants.Contains(variant)) {
                List<string> variants = ActiveVariants.ToList();
                variants.Add(variant);
                ActiveVariants = variants.ToArray();
            }
        }

        public void RemoveVariant(string variant) {
            if (ActiveVariants.Contains(variant)) {
                List<string> variants = ActiveVariants.ToList();
                variants.Remove(variant);
                ActiveVariants = variants.ToArray();
            }
        }

        /// <summary>
        /// 將 bundle name 改為正確的 variant
        /// </summary>
        /// <returns>The variant.</returns>
        /// <param name="bundle">Bundle.</param>
        public string RemapVariant(string bundle) {
            string[] split = bundle.Split('.');
            if (split.Length > 0)
                return _bundlesWithVariant.Where(x => x.Contains(split[0])).Where(x => ActiveVariants.Contains(x.Split('.')[1])).FirstOrDefault();
            return bundle;
        }
    }
}