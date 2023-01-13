using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RefreshPlay
{
    [InitializeOnLoadMethod]
    static void Run()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorApplication.ExecuteMenuItem("Assets/Refresh");
            }
        };
    }
}