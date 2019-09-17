#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEditor;


namespace AssetBundles.DataModel {
    public partial class Settings : ScriptableObject {

        public class Path {

            public static string DefaultBasePath => "Assets/";

            public static string BasePath => UserSettings.ConfigBaseDir;

            public static void ResetBasePath(string newPath) => UserSettings.ConfigBaseDir = newPath;

            public static string SettingDataPath => System.IO.Path.Combine(BasePath, "Settings.asset");

            public static string LocalBundlePath => $"file://{Application.dataPath.Replace("/Assets", "")}/AssetBundles/";
        }
    }
}
#endif