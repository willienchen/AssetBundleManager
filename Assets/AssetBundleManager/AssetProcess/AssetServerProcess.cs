using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace AssetBundles {
    public class AssetServerProcess : AssetManagerProcess {

        private AssetBundleManager abm;
        private Action<IEnumerator> coroutineHandler;

        public override string[] ActiveVariants { get => abm.Variant; set => abm.Variant = value; }

        //初始化
        public override IEnumerator Initialize(string[] uris) {
            abm = new AssetBundleManager();
            abm.SetBaseUri(uris);
            yield return abm.InitializeAsync();

            BundlesWithVariant = abm.Manifest.GetAllAssetBundlesWithVariant();
            Bundles = abm.Manifest.GetAllAssetBundles();

#if UNITY_EDITOR
            if (!Application.isPlaying)
                coroutineHandler = EditorCoroutine.Start;
            else
#endif
                coroutineHandler = AssetBundleDownloaderMonobehaviour.Instance.HandleCoroutine;
        }

        public override AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, LoadSceneMode mode) {
            var coroutine = new AssetBundleLoadLevelOperation(bundle, level, mode, abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override ILoadAssetOperation LoadAssetAsync<T>(string bundle, string asset) {
            var coroutine = new LoadAssetOperation(bundle, asset, typeof(T), abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override ILoadAssetOperation LoadAssetAsync(string bundle, string asset, Type type) {
            var coroutine = new LoadAssetOperation(bundle, asset, type, abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override ILoadMultiAssetOperation LoadAllAssetsAsync(string bundle) {
            var coroutine = new LoadAllAssetsOperation(bundle, abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override ILoadMultiAssetOperation LoadAssetWithSubAssetsAsync(string bundle, string asset, Type type) {
            var coroutine = new LoadSubAssetsOperation(bundle, asset, type, abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override ILoadMultiAssetOperation LoadMultiAssetOperation(string bundle, string[] asset, Type type) {
            var coroutine = new LoadMultiAssetsOperation(bundle, asset, type, abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override AssetBundleLoadOperation PreloadBundle(string bundle) {
            var coroutine = new AssetBundlePreLoadOperation(bundle, abm);
            coroutineHandler(coroutine);
            return coroutine;
        }

        public override bool IsCaching(string bundle, string variant = "") {
            if (!string.IsNullOrEmpty(variant)) {
                string[] dependencies = abm.Manifest.GetAllDependencies(bundle);
                for (int i = 0; i < dependencies.Length; i++) {
                    string depend = dependencies[i];
                    string[] curSplit = depend.Split('.');
                    if (curSplit.Length < 2) {
                        continue;
                    }

                    string depBundle = curSplit[0] + "." + variant;
                    if (!Caching.IsVersionCached(depBundle, abm.Manifest.GetAssetBundleHash(depBundle))) {
                        return false;
                    }
                }
            }
            return Caching.IsVersionCached(bundle, abm.Manifest.GetAssetBundleHash(bundle));
        }

        public override void UnloadBundle(string bundle, bool unloadAllLoadedObjects, bool force) => abm.UnloadBundle(bundle, unloadAllLoadedObjects, force);

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
