using System;
using gishadev.tools.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace gishadev.tools.Generative
{
    public class ToolsPostprocessor : AssetPostprocessor
    {
        private const string PACKAGE_IDENTIFIER = "gishadev-tools-polish";
        private const string PROCESSED_KEY = "GishaDevTools_EnumsGenerated";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            // Check if we've already processed this package
            if (EditorPrefs.GetBool(PROCESSED_KEY, false))
                return;

            bool packageDetected = false;

            foreach (string assetPath in importedAssets)
            {
                // Check if this is a package-related asset
                if (IsPackageAsset(assetPath))
                {
                    packageDetected = true;
                    break;
                }
            }

            if (packageDetected)
            {
                GenerateEnums();
                EditorPrefs.SetBool(PROCESSED_KEY, true);
            }
        }

        private static bool IsPackageAsset(string assetPath)
        {
            // Check if the asset path contains the package identifier
            return assetPath.Contains(PACKAGE_IDENTIFIER) ||
                   assetPath.Contains("Packages/com.gishadev.tools") || // More specific package path
                   assetPath.Contains("gishadev");
        }

        private static void GenerateEnums()
        {
            try
            {
                Debug.Log("Generating enums for Gisha Dev Tools Polish package...");

                string[] enumValues = Array.Empty<string>();

                EnumGenerator.GenerateEnumClass(Constants.AUDIO_MUSIC_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.AUDIO_SFX_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.POOL_SFX_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.POOL_VFX_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.POOL_OTHER_ENUM_NAME, enumValues);

                Debug.Log("Enums generated successfully!");

                // Refresh the asset database to ensure the new enums are recognized
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to generate enums: {ex.Message}");
            }
        }

        // Optional: Add a menu item to manually regenerate enums
        [MenuItem("Tools/Gisha Dev Tools/Regenerate Enums")]
        private static void RegenerateEnums()
        {
            EditorPrefs.SetBool(PROCESSED_KEY, false);
            GenerateEnums();
        }

        // Optional: Add a menu item to reset the processed flag
        [MenuItem("Tools/Gisha Dev Tools/Reset Package State")]
        private static void ResetPackageState()
        {
            EditorPrefs.DeleteKey(PROCESSED_KEY);
            Debug.Log("Package state reset. Enums will be regenerated on next import.");
        }
    }
}