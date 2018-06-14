using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildPreferences))]
public class BuildPreferencesEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildPreferences bp = (BuildPreferences)target;

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Build Controls", EditorStyles.boldLabel);
        if (bp.Targets != null && bp.Targets.Length > 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            foreach (BuildTarget buildTarget in bp.Targets)
            {
                if (GUILayout.Button("Build " + buildTarget.ToString()))
                {
                    BuilderEditorTools.BuildAny(buildTarget);
                }
            }

            EditorGUILayout.LabelField("");

            if (GUILayout.Button("Build All"))
            {
                BuilderEditorTools.BuildAllProjectTargets();
            }


            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
