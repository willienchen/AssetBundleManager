#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.AssetGraph;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

namespace AssetBundles {

    //AssetGraph bundle 處理
    public class AssetGraphProcess : AssetManagerProcess {

        public override IEnumerator Initialize(string[] uris) {
            ActiveVariants = new string[] { };
            Bundles = AssetBundleBuildMap.GetBuildMap().GetAllAssetBundleNames();
            BundlesWithVariant = Bundles.Where(x => x.Split('.').Length > 1).ToArray();
            yield break;
        }

        public override AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode) {
            return new Simulation.LoadLevelOperation(RemapVariant(bundle), level, mode);
        }

        public override ILoadAssetOperation LoadAssetAsync(string bundle, string asset, System.Type type) {
            var split = asset.Split('.');
            if (split.Length > 1) {
                asset = split[0];
            }

            string[] assetPaths = AssetBundleBuildMap.GetBuildMap().GetAssetPathsFromAssetBundleAndAssetName(RemapVariant(bundle), asset);

            if (assetPaths.Length == 0) {
                Debug.LogError($"AssetGraph process ,There is no asset with name \"{asset}\" in {RemapVariant(bundle)} , {type}");
                return null;
            }

            if (type.Equals(typeof(Sprite[]))) {
                type = typeof(Sprite);
            }

            UnityEngine.Object target;
            for (int i = 0; i < assetPaths.Length; i++) {
                target = AssetDatabase.LoadAssetAtPath(assetPaths[i], type);
                if (target != null) {
                    return new Simulation.LoadAssetOperation(target);
                }
            }

            Debug.LogError($"AssetGraph process , has Error ! bundle:{bundle} ,asset:{asset} ,type:{type}");
            return null;
        }

        public override ILoadMultiAssetOperation LoadAllAssetsAsync(string bundle) {
            string[] assetPaths = AssetBundleBuildMap.GetBuildMap().GetAssetPathsFromAssetBundle(bundle);
            return new Simulation.LoadMultiAssetOperation(assetPaths);
        }

        public override ILoadMultiAssetOperation LoadAssetWithSubAssetsAsync(string bundle, string asset, System.Type type) {
            return LoadAssetAsync(bundle, asset, type) as ILoadMultiAssetOperation;
        }

        public override ILoadMultiAssetOperation LoadMultiAssetOperation(string bundle, string[] asset, Type type) {
            return LoadAllAssetsAsync(bundle);
        }

        /// <summary>
        /// 將 bundle name 改為正確的 variant , 沒有找到則回傳 傳入值 , AssetGraph 要在這邊另外處理
        /// </summary>
        /// <returns>The variant.</returns>
        /// <param name="bundle">Bundle.</param>
        public string RemapVariant(string bundle) {
            string[] split = bundle.Split('.');
            if (split.Length > 1) {
                var result = BundlesWithVariant.Where(x => x.Contains(split[0])).Where(x => ActiveVariants.Contains(x.Split('.')[1])).FirstOrDefault();
                if (!string.IsNullOrEmpty(result)) {
                    return result;
                }
            }
            return bundle;
        }

        //讀取素材
        public override ILoadAssetOperation LoadAssetAsync<T>(string bundle, string asset) {
            return LoadAssetAsync(bundle, asset, typeof(T));
        }

        public override bool IsCaching(string bundle, string variant = "") => true;

        public override void Dispose() {
            BundlesWithVariant = null;
        }
    }
}
#endif