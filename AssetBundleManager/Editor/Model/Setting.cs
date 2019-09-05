using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using Unity.Collections;


namespace AssetBundles.DataModel {
    //儲存 AssetBundle 的設定
    public partial class Settings : ScriptableObject {

        [SerializeField] private int m_version;
        private const int VERSION = 1;

        private static Settings s_instance;

        //取得設定資料
        private static Settings GetSettings() {
            if (s_instance == null) {
                if (!Load()) {
                    //讀取失敗  建立一個 data asset
                    s_instance = ScriptableObject.CreateInstance<Settings>();
                    s_instance.m_version = VERSION;

                    var dir = Path.BasePath;

                    if (!Directory.Exists(dir)) {
                        Directory.CreateDirectory(dir);
                    }
                    AssetDatabase.CreateAsset(s_instance, Path.SettingDataPath);
                }
            }

            return s_instance;
        }

        private static bool Load() {
            bool loaded = false;

            try {
                var dbPath = Path.SettingDataPath;

                if (File.Exists(dbPath)) {

                    Settings db = AssetDatabase.LoadAssetAtPath<Settings>(dbPath);

                    if (db != null && db.m_version == VERSION) {

                        s_instance = db;
                        loaded = true;
                    }
                    else {
                        if (db != null) {
                            Resources.UnloadAsset(db);
                        }
                    }
                }
            }
            catch (System.Exception e) {
                Debug.LogException(e);
            }

            return loaded;
        }

        public static void SetSettingDirty() => EditorUtility.SetDirty(s_instance);

        [SerializeField] AssetBundleManagerMode m_mode;
        public static AssetBundleManagerMode Mode {
            get => GetSettings().m_mode;
            set {
                GetSettings().m_mode = value;
                SetSettingDirty();
            }
        }

        [SerializeField] string m_uri;
        public static string URI {
            get => GetSettings().m_uri;
            set {
                GetSettings().m_uri = value;
                SetSettingDirty();
            }
        }

        [SerializeField] string m_remote;
        public static string Remote {
            get => GetSettings().m_remote;
            set {
                GetSettings().m_remote = value;
                SetSettingDirty();
            }
        }
    }
}