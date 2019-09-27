using UnityEngine;
using System.Collections;

namespace AssetBundles {

    //public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation {
    //    public abstract T GetAsset<T>() where T : UnityEngine.Object;
    //    public abstract T[] GetAssets<T>() where T : UnityEngine.Object;
    //    public abstract Sprite GetSprite(string spriteName = null);
    //}

    public interface IAssetOperation {
        AssetBundleLoadOperation GetAsync();
        bool IsError();
    }

    public interface ILoadAssetOperation : IAssetOperation {
        T GetAsset<T>() where T : UnityEngine.Object;
    }

    public interface ILoadMultiAssetOperation : IAssetOperation {
        T GetAsset<T>(string asset) where T : UnityEngine.Object;
        T[] GetAssets<T>() where T : UnityEngine.Object;
    }
}
