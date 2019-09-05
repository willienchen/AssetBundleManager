using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace AssetBundles {

    public enum AssetBundleManagerMode : int {
        SimulationMode,
        SimulationModeGraphTool,
        Server
    }

    //處理取得資源管理
    public class AssetManager : System.IDisposable {

        public static AssetManager instance;

        //模式 初始化時設定
        public AssetBundleManagerMode mode;

        private AssetManagerProcess _process;


        public AssetManager() {
            if (instance != null) {
                return;
            }
            instance = this;
        }

        ~AssetManager() {
            instance = null;
        }

        public IEnumerator Initialize(string uri, AssetBundleManagerMode mode = AssetBundleManagerMode.Server) {
            return Initialize(new string[] { uri }, mode);
        }

        //初始化
        public IEnumerator Initialize(string[] uris, AssetBundleManagerMode mode = AssetBundleManagerMode.Server) {
#if UNITY_EDITOR
            if (mode == AssetBundleManagerMode.SimulationModeGraphTool) {
                _process = new AssetGraphProcess();
            }
            else
#endif
            {
                _process = new AssetServerProcess();
            }
            yield return _process.Initialize(uris);
        }

        public AssetBundleLoadOperation LoadLevelAsync(string bundle, string level, bool isAdditive) {
            return _process.LoadLevelAsync(bundle, level, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }

        public AssetBundleLoadAssetBaseOperation LoadAssetAsync<T>(string bundle, string asset) where T : UnityEngine.Object {
            return _process.LoadAssetAsync<T>(bundle, asset);
        }

        //新增 bundle 變數
        public void AddVariant(string variant) => _process.AddVariant(variant);

        //新增 bundle 變數
        public void RemoveVariant(string variant) => _process.RemoveVariant(variant);

        //資源釋放
        public void Dispose() {
            _process.Dispose();
            _process = null;
            Resources.UnloadUnusedAssets();
            instance = null;
        }
    }
}