using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AssetBundles {

    public abstract class AssetManagerProcess {

        public virtual string[] ActiveVariants { get; set; }

        public string[] BundlesWithVariant = null;   //初始化時 只紀錄 擁有 variant 的 bundle
        public string[] Bundles = null;             //初始化時記錄所有擁有的bundle

        //初始化
        public abstract IEnumerator Initialize(string[] uris);

        public abstract AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode);

        public abstract AssetBundleLoadAssetOperation LoadAssetAsync<T>(string bundle, string asset) where T : UnityEngine.Object;

        public abstract AssetBundleLoadAssetOperation LoadAssetAsync(string bundle, string asset, System.Type type);

        public virtual AssetBundleLoadOperation PreloadBundle(string bundle) { return null; }

        public virtual void UnloadBundle(string bundle, bool unloadAllLoadedObjects, bool force) { }

        protected virtual void UnloadVariantBundle(string variant) { }

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

        public void SwitchVariant(string from, string to) {
            Debug.Log("enter switch variant , from : " + from + " , to: " + to);
            if (ActiveVariants.Contains(from)) {
                RemoveVariant(from);
            }
            else {
                Debug.LogWarning("Asset Manager Process : Active variants not include " + from);
                UnloadVariantBundle(from);
            }
            AddVariant(to);
        }
    }
}