using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace AssetBundles {
    public class AssetServerProcess : AssetManagerProcess {

        private AssetBundleManager abm;

        //初始化
        public override IEnumerator Initialize(string[] uris) {
            abm = new AssetBundleManager();
            abm.SetBaseUri(uris);
            yield return abm.InitializeAsync();

            BundlesWithVariant = abm.Manifest.GetAllAssetBundlesWithVariant();
        }

        public override AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode) => new AssetBundleLoadLevelOperation(RemapVariant(bundle), level, mode, abm);

        public override AssetBundleLoadAssetOperation LoadAssetAsync<T>(string bundle, string asset) => new LoadAssetOperation(RemapVariant(bundle), asset, typeof(T), abm);

        public override void Dispose() {
            abm.Dispose();
            abm = null;
        }
    }
}
