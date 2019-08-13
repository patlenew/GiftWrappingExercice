using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor for the buttons in the inspector to generate the nodes
/// </summary>
[CustomEditor(typeof(TwoDimensionnalConvexHull))]
public class TwoDimensionalConvexHullEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TwoDimensionnalConvexHull instance = (TwoDimensionnalConvexHull)target;
        if (GUILayout.Button("Generate Gift Wrapping Algorithm", GUILayout.Height(50f)))
        {
            instance.GenerateNodes();
        }

        if (GUILayout.Button("Clean Gift Wrapping Algorithm", GUILayout.Height(50f)))
        {
            instance.CleanNodes();
        }
    }
}
