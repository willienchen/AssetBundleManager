using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace AssetBundles {

    //讀取 bundle 場景
    public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation {
        protected string m_LevelName;
        protected LoadSceneMode m_IsAdditive = LoadSceneMode.Additive;

        protected string m_DownloadingError;
        protected bool isError = false;
        protected AsyncOperation m_Request;

        public AssetBundleLoadLevelOperation(string bundle, string level, LoadSceneMode mode, AssetBundleManager manager) {
            m_LevelName = level;
            m_IsAdditive = mode;

            manager.GetBundle(bundle, OnAssetBundleComplete);
        }

        private void OnAssetBundleComplete(AssetBundle bundle) {
            if (bundle != null) {
                m_Request = SceneManager.LoadSceneAsync(m_LevelName, m_IsAdditive);
            }
            else {
                isError = true;
            }
        }

        public override bool IsDone() {
            if (m_Request == null && isError) {
                return true;
            }
            return m_Request != null && m_Request.isDone;
        }

        public override bool IsError() {
            return isError;
        }
    }
}