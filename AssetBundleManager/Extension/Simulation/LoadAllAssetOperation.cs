#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;

namespace AssetBundles.Simulation {

    public class LoadMultiAssetOperation : AssetBundleLoadOperation, ILoadMultiAssetOperation {
        private Object[] m_SimulatedObject;
        private Sprite[] m_Sprites;

        public LoadMultiAssetOperation(string[] assetsPath) {
            m_SimulatedObject = new Object[assetsPath.Length];
            for (int i = 0, max = assetsPath.Length; i < max; i++) {
                m_SimulatedObject[i] = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetsPath[i]);
            }
        }

        public AssetBundleLoadOperation GetAsync() => this;

        public T GetAsset<T>(string asset) where T : UnityEngine.Object {

            var t = m_SimulatedObject.FirstOrDefault(x => x.name == asset);

            if (t != null) {
                string path = AssetDatabase.GetAssetPath(t);
                return AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return null;
        }

        public T[] GetAssets<T>() where T : UnityEngine.Object {
            Debug.LogError("AssetBundles.Simulation.LoadMultiAssetOperation 尚未提供此服務");
            return m_SimulatedObject.Cast<T>().ToArray();
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