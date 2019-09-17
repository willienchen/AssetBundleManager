#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Linq;

namespace AssetBundles.Simulation {

    public class LoadAssetOperation : AssetBundleLoadAssetOperation {

        Object m_SimulatedObject;
        Sprite[] m_Sprites;

        public LoadAssetOperation(Object simulatedObject) {
            m_SimulatedObject = simulatedObject;
        }

        public override T GetAsset<T>() => m_SimulatedObject as T;

        public override T[] GetAssets<T>() {
            string path = UnityEditor.AssetDatabase.GetAssetPath(m_SimulatedObject);
            return UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path).OfType<T>().ToArray();
        }

        public override Sprite GetSprite(string spriteName = null) {
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