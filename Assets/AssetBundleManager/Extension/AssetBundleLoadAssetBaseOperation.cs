using UnityEngine;
using System.Collections;

namespace AssetBundles {

    public abstract class AssetBundleLoadAssetBaseOperation : AssetBundleLoadOperation {
        public abstract T GetAsset<T>() where T : UnityEngine.Object;
        public abstract T[] GetAssets<T>() where T : UnityEngine.Object;
        public abstract Sprite GetSprite(string spriteName = null);
    }
}
