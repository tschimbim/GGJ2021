using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BotScript))]
public class BotScriptEditor : Editor
{
    private BotScript botScript => (BotScript)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying && GUILayout.Button("Find New Target"))
            botScript.FindNewTarget();
    }
}
