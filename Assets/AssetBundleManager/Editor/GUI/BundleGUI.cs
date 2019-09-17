#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Settings = AssetBundles.DataModel.Settings;

namespace AssetBundles.Dev {

    public class BundleGUI : LabelOptionGUI {

        private string uri;
        private int index;
        private string[] options;

        public BundleGUI(EditorWindow window) : base(window) {
            uri = Settings.URI;
            index = (int)Settings.Mode;
            options = new string[] { "Local", "Graph", "Remote" };
        }

        protected override void DrawOptionGUI() {
            int option = GUILayout.Toolbar(index, options, GUILayout.Width(window.position.width - 120f), GUILayout.Height(50f));
            if (option != index) {
                index = option;
                UpdateBundleOption();
            }
        }

        protected override void DrawSubGUI() {
            if (Settings.Mode == AssetBundleManagerMode.Server) {
                GUILayout.BeginHorizontal();
                GUILayout.Space(120f);

                GUIStyle guiStyle = new GUIStyle();
                guiStyle.fontSize = 20;
                guiStyle.fixedWidth = 50f;

                GUILayout.Label("URI:", guiStyle);
                string defPath = uri;

                GUIStyle textFieldStyle = new GUIStyle("textfield");
                textFieldStyle.fontSize = 20;

                uri = GUILayout.TextField(defPath, textFieldStyle);

                if (uri != defPath) {
                    Settings.Remote = Settings.URI = uri;
                }
                GUILayout.EndHorizontal();
            }
        }

        private void UpdateBundleOption() {

            Settings.Mode = (AssetBundleManagerMode)index;

            if (Settings.Mode == AssetBundleManagerMode.SimulationMode) {
                //Simulate
                uri = Settings.URI = Settings.Path.LocalBundlePath;
            }
            else if (Settings.Mode == AssetBundleManagerMode.Server) {
                uri = Settings.URI = Settings.Remote;
            }
        }

        protected override string GetLabel() {
            return "Bundle Path";
        }

    }
}
#endif