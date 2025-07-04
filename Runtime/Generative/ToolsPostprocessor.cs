using System;
using gishadev.tools.Core;
using UnityEditor;
using Object = UnityEngine.Object;

namespace gishadev.tools.Generative
{
    public class ToolsPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                if (asset != null)
                {
                    if (asset.name.Contains("gishadev-tools-polish"))
                    {
                        string[] enumValues = Array.Empty<string>();
                        EnumGenerator.GenerateEnumClass(Constants.AUDIO_MUSIC_ENUM_NAME, enumValues);
                        EnumGenerator.GenerateEnumClass(Constants.AUDIO_SFX_ENUM_NAME, enumValues);
                        EnumGenerator.GenerateEnumClass(Constants.POOL_SFX_ENUM_NAME, enumValues);
                        EnumGenerator.GenerateEnumClass(Constants.POOL_VFX_ENUM_NAME, enumValues);
                        EnumGenerator.GenerateEnumClass(Constants.POOL_OTHER_ENUM_NAME, enumValues);
                    }
                }
            }
        }
    }
}