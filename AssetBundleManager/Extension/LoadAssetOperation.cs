using UnityEngine;
using System.Collections;
using System.Linq;

namespace AssetBundles {

    public class LoadAssetOperation : AssetBundleLoadAssetOperation {

        protected string m_AssetBundleName;
        protected string m_AssetName;
        protected bool m_isError = false;
        protected System.Type m_Type;
        protected AssetBundleRequest m_Request = null;

        protected bool m_isDone;
        protected AssetBundle m_bundle;

        public LoadAssetOperation(string bundle, string assetName, System.Type type, AssetBundleManager manager) {
            m_AssetBundleName = bundle;
            m_AssetName = assetName;
            m_Type = type;
            m_isDone = false;
            m_isError = false;

            //Debug.Log($"Load Asset Operation : bundle path:" + bundle + " , asset name :" + assetName);

            manager.GetBundle(bundle, OnAssetBundleComplete);
        }

        public override T GetAsset<T>() => (m_Request != null && m_Request.isDone) ? m_Request.asset as T : null;

        public override T[] GetAssets<T>() => (m_Request != null && m_Request.isDone) ? m_bundle.LoadAssetWithSubAssets<T>(m_AssetName) : null;

        //public override Sprite[] GetSprites() => m_bundle != null ? m_bundle.LoadAssetWithSubAssets<Sprite>(m_AssetName) : null;

        public override Sprite GetSprite(string spriteName = null) {
            spriteName = spriteName ?? m_AssetName;
            return GetAssets<UnityEngine.Sprite>().FirstOrDefault(x => x.name == spriteName);
        }

        private void OnAssetBundleComplete(AssetBundle bundle) {
            m_bundle = bundle;

            if (bundle != null) {
                ///@TODO: When asset bundle download fails this throws an exception...
                if (string.IsNullOrEmpty(m_AssetName)) {
                    Debug.LogError("AssetName is null " + m_AssetName);
                    m_isError = true;
                    return;
                }
                m_Request = bundle.LoadAssetAsync(m_AssetName, m_Type);
            }
            else {
                m_isError = true;
            }
        }

        public override bool IsDone() => (m_Request == null && m_isError) ? true : m_Request != null && m_Request.isDone;

        public override bool IsError() => m_isError;
    }
}