#if UNITY_EDITOR 
using UnityEngine;
//using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections;

namespace AssetBundles.Simulation {

    public class LoadLevelOperation : AssetBundleLoadOperation {
        AsyncOperation m_Operation = null;
        bool m_isError;

        public LoadLevelOperation(string bundle, string level, UnityEngine.SceneManagement.LoadSceneMode mode) {
            string[] levelPaths = null;
            //if (Settings.Mode == Settings.AssetBundleManagerMode.SimulationModeGraphTool) {
            levelPaths = UnityEngine.AssetGraph.AssetBundleBuildMap.GetBuildMap().GetAssetPathsFromAssetBundleAndAssetName(bundle, level);
            //}
            //else {
            //    levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);
            //}

            //levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, levelName);

            if (levelPaths.Length == 0) {
                ///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
                //        from that there right scene does not exist in the asset bundle...

                Debug.LogError("There is no scene with name \"" + level + "\" in " + bundle);
                m_isError = true;
                return;
            }

            m_Operation = EditorSceneManager.LoadSceneAsyncInPlayMode(levelPaths[0], new UnityEngine.SceneManagement.LoadSceneParameters(mode));
        }

        public override bool IsDone() {
            return m_Operation == null || m_Operation.isDone;
        }

        public override bool IsError() {
            return m_isError;
        }
    }
}
#endif