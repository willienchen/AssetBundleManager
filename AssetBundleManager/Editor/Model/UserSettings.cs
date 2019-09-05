using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AssetBundles.DataModel {

    public partial class Settings {

        public static class UserSettings {

            private const string PREFKEY_CONFIG_BASE_DIR = "AssetBundles.Test.ConfigBaseDir";

            public static string ConfigBaseDir {
                get {
                    var baseDir = EditorUserSettings.GetConfigValue(PREFKEY_CONFIG_BASE_DIR);
                    if (string.IsNullOrEmpty(baseDir)) {
                        return System.IO.Path.Combine(Path.DefaultBasePath, "BundleTest");
                    }
                    return baseDir;
                }

                set {
                    EditorUserSettings.SetConfigValue(PREFKEY_CONFIG_BASE_DIR, value);
                }
            }
        }
    }
}