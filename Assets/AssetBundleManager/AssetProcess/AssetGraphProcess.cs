using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.AssetGraph;
using UnityEngine.SceneManagement;

namespace AssetBundles {

    //AssetGraph bundle 處理
    public class AssetGraphProcess : AssetManagerProcess {

        public override IEnumerator Initialize(string[] uris) {
            _bundlesWithVariant = AssetBundleBuildMap.GetBuildMap().GetAllAssetBundleNames();
            yield break;
        }

        public override AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode) {
            return new Simulation.LoadLevelOperation(RemapVariant(bundle), level, mode);
        }

        //讀取素材
        public override AssetBundleLoadAssetBaseOperation LoadAssetAsync<T>(string bundle, string asset) {

            string[] assetPaths = AssetBundleBuildMap.GetBuildMap().GetAssetPathsFromAssetBundleAndAssetName(RemapVariant(bundle), asset);

            if (assetPaths.Length == 0) {
                Debug.LogError($"AssetGraph process ,There is no asset with name \"{asset}\" in {bundle}");
                return null;
            }

            Object target;
            for (int i = 0; i < assetPaths.Length; i++) {
                target = AssetDatabase.LoadAssetAtPath(assetPaths[i], typeof(T));
                if (target != null) {
                    return new Simulation.LoadAssetOperation(target);
                }
            }

            Debug.LogError($"AssetGraph process , has Error ! bundle:{bundle},asset:{asset},type:{typeof(T)}");
            return null;
        }

        public override void Dispose() {
            //Asset Graph 不需要 dispose 
        }
    }
}