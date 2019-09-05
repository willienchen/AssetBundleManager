using UnityEngine;
using System.Collections;

namespace AssetBundles {

    //預載 bundle  使用 , 不處理任何事情
    public class AssetBundlePreLoadOperation : AssetBundleLoadOperation {
        private bool isDone = false;
        private bool _isError = false;

        public AssetBundlePreLoadOperation(string bundle, AssetBundleManager manager) {
            isDone = false;
            manager.GetBundle(bundle, OnAssetBundleComplete);
        }

        private void OnAssetBundleComplete(AssetBundle bundle) {
            if (bundle == null) {
                _isError = true;
            }
            isDone = bundle != null;
        }

        public override bool IsDone() {
            return isDone || _isError;
        }

        public override bool IsError() {
            return _isError;
        }
    }
}