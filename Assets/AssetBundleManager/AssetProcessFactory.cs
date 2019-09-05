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
    public class AssetProcessFactory {

        public AssetManagerProcess _process;

        public string[] URIs;
        public System.Action<AssetManagerProcess> CallBack;
        public AssetBundleManagerMode Mode;

        public AssetProcessFactory(string uri = "", AssetBundleManagerMode mode = AssetBundleManagerMode.Server, System.Action<AssetManagerProcess> callback = null) {
            this.URIs = new string[] { uri };
            this.CallBack = callback;
            this.Mode = mode;
        }

        //初始化
        public IEnumerator Initialize() {

            AssetManagerProcess process;

#if UNITY_EDITOR
            if (Mode == AssetBundleManagerMode.SimulationModeGraphTool) {
                process = new AssetGraphProcess();
            }
            else
#endif
            {
                process = new AssetServerProcess();
            }
            yield return process.Initialize(URIs);

            if (this.CallBack != null)
                CallBack(process);
        }
    }
}