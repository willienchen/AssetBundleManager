using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AssetBundles {
    //同一個 bundle 內讀取多張圖
    public class LoadMultiAssetsOperation : AssetBundleOperation, ILoadMultiAssetOperation {

        protected string[] _assets;
        protected System.Type _type;

        protected List<AssetBundleRequest> _requests;
        protected Object[] _objects;

        private bool _isProcess = false;

        public LoadMultiAssetsOperation(string bundleName, string[] assets, System.Type type, AssetBundleManager manager) : base(bundleName, manager) {
            this._assets = assets;
            this._type = type;

            if (_assets.Length <= 0) {
                _isError = true;
            }
            else {
                GetBundle();
            }
        }

        public T GetAsset<T>(string asset) where T : UnityEngine.Object {
            return _objects.FirstOrDefault(x => x.name == asset) as T;
        }

        public T[] GetAssets<T>() where T : UnityEngine.Object {
            return IsDone() ? _objects.Cast<T>().ToArray() : null;
        }

        protected override AsyncOperation GenerateRequest() => null;

        protected override void OnAssetBundleComplete(AssetBundle bundle) {
            if (bundle == null) return;
            _requests = new List<AssetBundleRequest>();

            for (int i = 0, max = _assets.Length; i < max; i++) {
                _requests.Add(bundle.LoadAssetAsync(_assets[i], _type));
            }
        }

        public override bool IsDone() {
            if (_isError) return true;

            if (_requests != null) {
                //判斷是否還有未完成的
                if (_requests.FirstOrDefault(x => !x.isDone) == null) {
                    //僅執行一次
                    if (!_isProcess) {
                        _objects = _requests.Select(x => x.asset).ToArray();
                        _isProcess = true;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}