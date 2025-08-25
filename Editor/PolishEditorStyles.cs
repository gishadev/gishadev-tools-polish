using System;
using UnityEngine;

namespace gishadev.tools.Editor
{
    public class PolishEditorStyles
    {
        public void CreateBigButton(string label, Action action)
        {
            GUIStyle bigBoldButton = new GUIStyle(GUI.skin.button);
            bigBoldButton.fontSize = 14;
            bigBoldButton.fontStyle = FontStyle.Bold; 
            bigBoldButton.fixedHeight = 40;   
            
            if (GUILayout.Button(label, bigBoldButton)) 
                action?.Invoke();
        }
    }
}