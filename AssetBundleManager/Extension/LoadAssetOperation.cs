using UnityEngine;
using System.Collections;
using System.Linq;

namespace AssetBundles {

    public class LoadAssetOperation : AssetBundleOperation, ILoadAssetOperation {
        protected string _assetName;
        protected System.Type _type;
        protected new AssetBundleRequest _request = null;

        protected bool m_isDone;
        protected AssetBundle _bundle;
        protected AssetBundleManager _abm;

        public LoadAssetOperation(string bundle, string assetName, System.Type type, AssetBundleManager manager) : base(bundle, manager) {
            _bundleName = bundle;
            _assetName = assetName;
            _type = type;
            _abm = manager;
            //Debug.Log($"Load Asset Operation : bundle path:" + bundle + " , asset name :" + assetName);
            GetBundle();
        }

        protected override AsyncOperation GenerateRequest() => _request = _assetBundle.LoadAssetAsync(_assetName, _type);


        public T GetAsset<T>() where T : UnityEngine.Object => (_request != null && _request.isDone) ? _request.asset as T : null;


        //public override T[] GetAssets<T>() {
        //    return (_request != null && _request.isDone && _bundle != null) ? _bundle.LoadAssetWithSubAssets<T>(_assetName) : null;
        //}

        //public override Sprite[] GetSprites() => m_bundle != null ? m_bundle.LoadAssetWithSubAssets<Sprite>(m_AssetName) : null;

        //public override Sprite GetSprite(string spriteName = null) {
        //    spriteName = spriteName ?? _assetName;
        //    Sprite[] sprites = GetAssets<UnityEngine.Sprite>();
        //    if (sprites != null) {
        //        return sprites.FirstOrDefault(x => x.name == spriteName);
        //    }
        //    return null;
        //}

        //private void OnAssetBundleComplete(AssetBundle bundle) {
        //    _bundle = bundle;

        //    if (bundle != null) {
        //        ///@TODO: When asset bundle download fails this throws an exception...
        //        if (string.IsNullOrEmpty(_assetName)) {
        //            Debug.LogError("AssetName is null " + _assetName);
        //            _isError = true;
        //            return;
        //        }
        //        _request = bundle.LoadAssetAsync(_assetName, _type);
        //    }
        //    else {
        //        _isError = true;
        //    }
        //}

        //public override bool IsDone() {
        //    if (_isError)
        //        return true;
        //    else if (_request != null) {
        //        if (_request.isDone && _bundle != null) {
        //            _abm.UnloadBundle(_bundle);
        //            _bundle = null;
        //        }
        //        return _request.isDone;
        //    }
        //    return (_request == null && _isError) ? true : _request != null && _request.isDone;
        //}

        //public override bool IsError() => _isError;
    }
}