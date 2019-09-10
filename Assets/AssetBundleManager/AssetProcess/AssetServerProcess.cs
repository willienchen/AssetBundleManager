using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace AssetBundles {
    public class AssetServerProcess : AssetManagerProcess {

        private AssetBundleManager abm;

        public override string[] ActiveVariants { get => abm.Variant; set => abm.Variant = value; }

        //初始化
        public override IEnumerator Initialize(string[] uris) {
            abm = new AssetBundleManager();
            abm.SetBaseUri(uris);
            yield return abm.InitializeAsync();

            BundlesWithVariant = abm.Manifest.GetAllAssetBundlesWithVariant();
            Bundles = abm.Manifest.GetAllAssetBundles();
        }

        public override AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode) => new AssetBundleLoadLevelOperation(bundle, level, mode, abm);

        public override AssetBundleLoadAssetOperation LoadAssetAsync<T>(string bundle, string asset) => new LoadAssetOperation(bundle, asset, typeof(T), abm);

        public override AssetBundleLoadAssetOperation LoadAssetAsync(string bundle, string asset, Type type) => new LoadAssetOperation(bundle, asset, type, abm);

        public override void UnloadBundle(string bundle, bool unloadAllLoadedObjects, bool force) => abm.UnloadBundle(bundle, unloadAllLoadedObjects, force);

        public override AssetBundleLoadOperation PreloadBundle(string bundle) => new AssetBundlePreLoadOperation(bundle, abm);

        protected override void UnloadVariantBundle(string variant) {
            abm.UnloadVariantBundle(variant);
            Resources.UnloadUnusedAssets();
        }

        public override void Dispose() {
            BundlesWithVariant = null;
            abm.Dispose();
            abm = null;

        }
    }
}
