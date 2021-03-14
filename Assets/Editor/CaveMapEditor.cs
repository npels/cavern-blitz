using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CaveMap))]
public class CaveMapEditor : Editor {

    CaveMap caveMap;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DrawSettingsEditor(caveMap.settings, caveMap.GenerateCave);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated) {
        using (var check = new EditorGUI.ChangeCheckScope()) {
            Editor editor = CreateEditor(settings);
            editor.OnInspectorGUI();

            if (check.changed) {
                if (onSettingsUpdated != null && caveMap.autoUpdateInEditor) {
                    onSettingsUpdated();
                }
            }
        }
    }

    private void OnEnable() {
        caveMap = (CaveMap)target;
    }
}
