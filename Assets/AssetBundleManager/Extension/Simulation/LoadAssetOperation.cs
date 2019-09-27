#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Linq;

namespace AssetBundles.Simulation {

    public class LoadAssetOperation : AssetBundleLoadOperation, ILoadAssetOperation, ILoadMultiAssetOperation {

        Object m_SimulatedObject;
        Sprite[] m_Sprites;

        public LoadAssetOperation(Object simulatedObject) {
            m_SimulatedObject = simulatedObject;
        }

        public AssetBundleLoadOperation GetAsync() => this;

        public T GetAsset<T>() where T : UnityEngine.Object => m_SimulatedObject as T;

        public T[] GetAssets<T>() where T : UnityEngine.Object {
            string path = UnityEditor.AssetDatabase.GetAssetPath(m_SimulatedObject);
            return UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path).OfType<T>().ToArray();
        }

        public T GetAsset<T>(string asset) where T : UnityEngine.Object {
            return GetAsset<T>();
        }

        public Sprite GetSprite(string spriteName = null) {
            spriteName = spriteName ?? m_SimulatedObject.name;
            return GetAssets<UnityEngine.Sprite>().FirstOrDefault(x => x.name == spriteName);
        }

        public override bool IsDone() {
            return true;
        }

        public override bool IsError() {
            return false;
        }
    }
}
#endif