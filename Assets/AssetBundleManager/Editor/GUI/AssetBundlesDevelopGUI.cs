using UnityEngine;
using System.Collections;
using UnityEditor;
using Settings = AssetBundles.DataModel.Settings;

namespace AssetBundles.Dev {

    //開發用 GUI
    public class AssetBundlesDevelopGUI : EditorWindow {

        private static AssetBundlesDevelopGUI s_instance = null;
        internal static AssetBundlesDevelopGUI instance {
            get {
                if (s_instance == null)
                    s_instance = GetWindow<AssetBundlesDevelopGUI>();
                return s_instance;
            }
        }

        [MenuItem("Window/AssetBundle Test", priority = 2051)]
        static void ShowWindow() {
            s_instance = null;
            instance.titleContent = new GUIContent("AssetBundles Test");
            instance.maxSize = new Vector2(400f, 200f);
            instance.Show();
        }

        private BundleGUI m_bundleGUI;

        private void OnEnable() {
            m_bundleGUI = new BundleGUI(this);
        }

        private void OnGUI() {

            EditorGUILayout.Space();

            m_bundleGUI.OnGUI();
        }
    }
}
