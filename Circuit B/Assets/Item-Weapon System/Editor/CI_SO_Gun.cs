using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_Weapon_Ranged), true)]
public class CI_SO_Gun : CI_SO_Weapon
{
    SerializedProperty spReloadSpeed;
    SerializedProperty spMaxAmmoInClip;
    SerializedProperty spFireRange;
    SerializedProperty spFireSpeed;
    SerializedProperty spRecoilAmount;
    public override void OnEnable()
    {
        base.OnEnable();
        spReloadSpeed = serializedObject.FindProperty("reloadSpeed");
        spMaxAmmoInClip = serializedObject.FindProperty("maxAmmoInclip");
        spFireRange = serializedObject.FindProperty("fireRange");
        spFireSpeed = serializedObject.FindProperty("fireSpeed");
        spRecoilAmount = serializedObject.FindProperty("recoilAmount");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.LabelField("Reload Speed:", headerStyle);
        EditorGUILayout.LabelField(spReloadSpeed.floatValue.ToString(),textStyle);
        EditorGUILayout.LabelField("Max Ammo In Clip:", headerStyle);
        EditorGUILayout.LabelField(spMaxAmmoInClip.intValue.ToString(), textStyle);
        EditorGUILayout.LabelField("Fire Range:", headerStyle);
        EditorGUILayout.LabelField(spFireRange.floatValue.ToString(), textStyle);
        EditorGUILayout.LabelField("Fire Speed:", headerStyle);
        EditorGUILayout.LabelField(spFireSpeed.floatValue.ToString(), textStyle);
        EditorGUILayout.LabelField("Recoil Amount:", headerStyle);
        EditorGUILayout.LabelField(spRecoilAmount.floatValue.ToString(), textStyle);
        serializedObject.ApplyModifiedProperties();
    }
}
