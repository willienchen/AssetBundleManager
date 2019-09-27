using UnityEngine;
using System.Collections;
using System.Linq;

namespace AssetBundles {
    //讀取 bundle 同一個 Asset 底下的 素材
    public class LoadSubAssetsOperation : AssetBundleOperation, ILoadMultiAssetOperation {

        protected string _assetName;
        protected System.Type _type;
        protected new AssetBundleRequest _request;

        public LoadSubAssetsOperation(string bundleName, string asset, System.Type type, AssetBundleManager manager) : base(bundleName, manager) {
            _assetName = asset;
            _type = type;
            GetBundle();
        }

        public T GetAsset<T>(string asset) where T : UnityEngine.Object {
            return _request.allAssets.FirstOrDefault(x => x.name == asset) as T;
        }

        public T[] GetAssets<T>() where T : UnityEngine.Object {
            return IsDone() ? _request.allAssets.Cast<T>().ToArray() : null;
        }

        protected override AsyncOperation GenerateRequest() {
            return _request = _assetBundle.LoadAssetWithSubAssetsAsync(_assetName, _type);
        }

        protected override void OnAssetBundleComplete(AssetBundle bundle) {
            if (string.IsNullOrEmpty(_assetName)) {
                Debug.LogError("AssetName is null " + _assetName);
                _isError = true;
                return;
            }
            base.OnAssetBundleComplete(bundle);
        }
    }
}