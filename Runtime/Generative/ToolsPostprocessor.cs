using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using gishadev.tools.Core;

namespace gishadev.tools.Generative
{
    [InitializeOnLoad]
    public static class ToolsEnumsInitializer
    {
        private const string PACKAGE_NAME = "gishadev-tools-polish";
        private const string PROCESSED_KEY = "GishaDevTools_EnumsGenerated";
        private const string VERSION_KEY = "GishaDevTools_Version";

        static ToolsEnumsInitializer()
        {
            // Use EditorApplication.delayCall to ensure Unity is fully initialized
            EditorApplication.delayCall += CheckForPackageImport;
        }

        private static void CheckForPackageImport()
        {
            try
            {
                // Method 1: Check if this is the first time running after package import
                if (!EditorPrefs.GetBool(PROCESSED_KEY, false))
                {
                    if (IsPackagePresent())
                    {
                        Debug.Log("Gisha Dev Tools package detected for the first time!");
                        GenerateEnums();
                        EditorPrefs.SetBool(PROCESSED_KEY, true);
                        return;
                    }
                }

                // Method 2: Check for version changes (if package was updated)
                string currentVersion = GetPackageVersion();
                string savedVersion = EditorPrefs.GetString(VERSION_KEY, "");

                if (!string.IsNullOrEmpty(currentVersion) && currentVersion != savedVersion)
                {
                    Debug.Log($"Package version changed from {savedVersion} to {currentVersion}");
                    GenerateEnums();
                    EditorPrefs.SetString(VERSION_KEY, currentVersion);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error checking for package import: {ex.Message}");
            }
        }

        private static bool IsPackagePresent()
        {
            // Check multiple possible locations
            string[] possiblePaths =
            {
                "Packages/com.gishadev.tools/package.json",
                "Packages/com.gishadev.tools-polish/package.json",
                "Assets/GishaDevTools/package.json"
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    Debug.Log($"Found package at: {path}");
                    return true;
                }
            }

            // Alternative: Check for specific assets
            string[] guids = AssetDatabase.FindAssets("t:Script", new[] { "Packages", "Assets" });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Contains(PACKAGE_NAME) || path.Contains("GishaDevTools"))
                {
                    Debug.Log($"Found package script at: {path}");
                    return true;
                }
            }

            return false;
        }

        private static string GetPackageVersion()
        {
            string[] possiblePaths =
            {
                "Packages/com.gishadev.tools/package.json",
                "Packages/com.gishadev.tools-polish/package.json",
                "Assets/GishaDevTools/package.json"
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        string json = File.ReadAllText(path);
                        // Simple version extraction (you might want to use proper JSON parsing)
                        var versionStart = json.IndexOf("\"version\":");
                        if (versionStart != -1)
                        {
                            var versionEnd = json.IndexOf(",", versionStart);
                            if (versionEnd == -1) versionEnd = json.IndexOf("}", versionStart);
                            var versionString = json.Substring(versionStart, versionEnd - versionStart);
                            return versionString;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Could not read version from {path}: {ex.Message}");
                    }
                }
            }

            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Fallback to timestamp
        }

        private static void GenerateEnums()
        {
            try
            {
                Debug.Log("Generating enums for Gisha Dev Tools package...");

                string[] enumValues = Array.Empty<string>();

                EnumGenerator.GenerateEnumClass(Constants.AUDIO_MUSIC_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.AUDIO_SFX_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.POOL_SFX_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.POOL_VFX_ENUM_NAME, enumValues);
                EnumGenerator.GenerateEnumClass(Constants.POOL_OTHER_ENUM_NAME, enumValues);

                Debug.Log("Enums generated successfully!");

                // Refresh the asset database
                AssetDatabase.Refresh();

                // Optional: Show a dialog to inform the user
                EditorUtility.DisplayDialog("Gisha Dev Tools",
                    "Package imported successfully!\nEnums have been generated.", "OK");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to generate enums: {ex.Message}");
                EditorUtility.DisplayDialog("Error",
                    $"Failed to generate enums: {ex.Message}", "OK");
            }
        }

        // Manual controls via menu
        [MenuItem("Tools/Gisha Dev Tools/Force Generate Enums")]
        private static void ForceGenerateEnums()
        {
            GenerateEnums();
        }

        [MenuItem("Tools/Gisha Dev Tools/Reset Package Detection")]
        private static void ResetPackageDetection()
        {
            EditorPrefs.DeleteKey(PROCESSED_KEY);
            EditorPrefs.DeleteKey(VERSION_KEY);
            Debug.Log(
                "Package detection reset. Enums will be regenerated on next Unity restart or script recompilation.");
        }

        [MenuItem("Tools/Gisha Dev Tools/Check Package Status")]
        private static void CheckPackageStatus()
        {
            bool isProcessed = EditorPrefs.GetBool(PROCESSED_KEY, false);
            string version = EditorPrefs.GetString(VERSION_KEY, "Unknown");
            bool isPresent = IsPackagePresent();

            string message = $"Package Present: {isPresent}\n" +
                             $"Enums Generated: {isProcessed}\n" +
                             $"Version: {version}";

            EditorUtility.DisplayDialog("Package Status", message, "OK");
        }
    }
}