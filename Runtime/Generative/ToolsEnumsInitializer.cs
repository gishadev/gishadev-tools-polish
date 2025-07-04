using UnityEngine;
using UnityEditor;

namespace gishadev.tools.Generative
{
    [InitializeOnLoad]
    public static class ToolsEnumsInitializer
    {
        static ToolsEnumsInitializer()
        {
            Debug.Log("Custom package loaded in Editor!");
        }
    }
}