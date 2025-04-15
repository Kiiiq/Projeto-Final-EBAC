using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CharacterStateMachine))]
public class CharacterSMEditor : Editor
{
    public bool showFoldOut;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CharacterStateMachine example = (CharacterStateMachine)target;

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("State Machine");

        if (example.stateMachine == null)
        {

            EditorGUILayout.LabelField("State Machine is null");
            return;
        }

        if (example.stateMachine.getCurrentState() != null)
        {

            EditorGUILayout.LabelField("Current State", example.stateMachine.getCurrentState().ToString());

        }
        else
        {
            EditorGUILayout.LabelField("Current State --> null");
        }

        showFoldOut = EditorGUILayout.Foldout(showFoldOut, "Possible States");

        if (showFoldOut)
        {
            var keys = example.stateMachine.stateDictionary.Keys.ToArray();
            var values = example.stateMachine.stateDictionary.Values.ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                EditorGUILayout.LabelField(string.Format("{0} --> {1}", keys[i], values[i]));
            }


        }
    }
}
