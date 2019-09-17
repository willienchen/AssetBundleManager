#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace AssetBundles.Dev {

    public abstract class LabelOptionGUI {

        protected EditorWindow window;

        public LabelOptionGUI(EditorWindow window) {
            this.window = window;
        }

        public void OnGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label(GetLabel(), "LargeLabel", GUILayout.Width(100f));

            DrawOptionGUI();

            GUILayout.EndHorizontal();

            DrawSubGUI();

            GUILayout.Space(10f);
        }

        /// <summary>
        /// 子選項
        /// </summary>
        protected abstract void DrawOptionGUI();

        protected virtual void DrawSubGUI() { }

        protected abstract string GetLabel();
    }
}
#endif