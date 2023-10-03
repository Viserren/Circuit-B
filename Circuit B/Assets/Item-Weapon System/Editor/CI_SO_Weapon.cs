using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_Weapon), true)]
public class CI_SO_Weapon : CI_SO_Item
{
    SerializedProperty spDamage;
    public override void OnEnable()
    {
        base.OnEnable();
        spDamage = serializedObject.FindProperty("damage");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.LabelField("Damage:", headerStyle);
        EditorGUILayout.LabelField(spDamage.floatValue.ToString(), textStyle);
        serializedObject.ApplyModifiedProperties();
    }
}
