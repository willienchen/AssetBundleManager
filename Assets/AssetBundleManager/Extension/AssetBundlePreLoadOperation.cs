using UnityEngine;
using System.Collections;

namespace AssetBundles {

    //預載 bundle  使用 , 不處理任何事情
    //base._assetBundle always null
    public class AssetBundlePreLoadOperation : AssetBundleOperation {
        private bool _isDone = false;

        public AssetBundlePreLoadOperation(string bundle, AssetBundleManager manager) : base(bundle, manager) {
            _isDone = false;
            Hash128 hash = manager.Manifest.GetAssetBundleHash(bundle);

            //在 caching 內 就不用動作了
            if (Caching.IsVersionCached(bundle, hash)) {
                _isDone = true;
            }
            else {
                GetBundle();
            }
        }

        protected override AsyncOperation GenerateRequest() => null;

        protected override void OnAssetBundleComplete(AssetBundle bundle) {
            if (bundle == null) {
                _isError = true;
                return;
            }
            _isDone = true;
            _manager.UnloadBundle(bundle);
        }

        public override bool IsDone() => _isDone || _isError;

    }
}