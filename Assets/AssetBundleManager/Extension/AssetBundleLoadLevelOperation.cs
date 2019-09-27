using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace AssetBundles {
    //讀取 bundle 場景
    public class AssetBundleLoadLevelOperation : AssetBundleOperation {
        protected string _levelName;
        protected LoadSceneMode _isAdditive = LoadSceneMode.Additive;

        public AssetBundleLoadLevelOperation(string bundle, string level, LoadSceneMode mode, AssetBundleManager manager) : base(bundle, manager) {
            _levelName = level;
            _isAdditive = mode;

            GetBundle();
        }

        protected override AsyncOperation GenerateRequest() {
            return _request = SceneManager.LoadSceneAsync(_levelName, _isAdditive);
        }
    }
}