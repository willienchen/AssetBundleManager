using UnityEngine;
using System.Collections;
using System.Linq;

namespace AssetBundles {
    //讀取 bundle 相同 Type 的多個素材
    public class LoadAllAssetsOperation : AssetBundleOperation, ILoadMultiAssetOperation {

        protected new AssetBundleRequest _request;

        public LoadAllAssetsOperation(string bundle, AssetBundleManager manager) : base(bundle, manager) {
            GetBundle();
        }

        protected override AsyncOperation GenerateRequest() => _request = _assetBundle.LoadAllAssetsAsync();


        public T GetAsset<T>(string asset) where T : UnityEngine.Object {
            return _request.allAssets.Where(x => x.name == asset).FirstOrDefault(x => x.GetType().Equals(typeof(T))) as T;
        }

        public T[] GetAssets<T>() where T : UnityEngine.Object => IsDone() ? _request.allAssets.Cast<T>().ToArray() : null;
    }
}