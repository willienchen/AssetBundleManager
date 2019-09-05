using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
namespace AssetBundles.Dev {
    public class AssetDevPostprocesor : UnityEditor.AssetPostprocessor {

        //导入，删除，移动，都会调用此方法，注意，这个方法是static的
        static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            for (int i = 0; i < movedFromAssetPaths.Length; i++) {
                if (movedFromAssetPaths[i] == AssetBundles.DataModel.Settings.Path.BasePath) {
                    AssetBundles.DataModel.Settings.Path.ResetBasePath(movedAssets[i]);
                }
            }
        }
    }
}
#endif