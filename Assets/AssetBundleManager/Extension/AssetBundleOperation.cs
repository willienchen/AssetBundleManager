using UnityEngine;
using System.Collections;

namespace AssetBundles {
    public abstract class AssetBundleOperation : AssetBundleLoadOperation {
        protected string _bundleName;

        protected AssetBundle _assetBundle;
        protected AssetBundleManager _manager;
        protected AsyncOperation _request = null;

        public AssetBundleOperation(string bundleName, AssetBundleManager manager) {
            _bundleName = bundleName;
            _manager = manager;
            _isError = false;
        }

        public AssetBundleLoadOperation GetAsync() => this;

        protected void GetBundle() {
            _manager.GetBundle(_bundleName, OnAssetBundleComplete);
        }

        protected abstract AsyncOperation GenerateRequest();

        protected virtual void OnAssetBundleComplete(AssetBundle bundle) {
            _assetBundle = bundle;
            if (!(_isError = _assetBundle == null)) {
                _request = GenerateRequest();
            }
        }

        public void UnloadBundle() {
            if (_assetBundle != null) {
                _manager.UnloadBundle(_assetBundle);
                _assetBundle = null;
            }
        }

        public override bool IsDone() {
            if (_isError)
                return true;

            if (_request != null) {
                if (_request.isDone && !AssetManagerProcess.KeepBundle) {
                    UnloadBundle();
                }
                return _request.isDone;
            }

            return false;
        }

        public override bool IsError() => _isError;
    }
}

