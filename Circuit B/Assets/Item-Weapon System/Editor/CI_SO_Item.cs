using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(SO_Item), true)]
public class CI_SO_Item : Editor
{
    SerializedProperty spName;
    SerializedProperty spDescription;
    SerializedProperty spSprite;
    SerializedProperty spItemId;
    public GUIStyle headerStyle { get; private set; }
    public GUIStyle textStyle { get; private set; }
    public virtual void OnEnable()
    {
        headerStyle = new GUIStyle();
        headerStyle.fontSize = 22;
        headerStyle.normal.textColor = Color.white;
        headerStyle.fontStyle = FontStyle.Bold;

        textStyle = new GUIStyle();
        textStyle.fontSize = 16;
        textStyle.normal.textColor = Color.white;

        spName = serializedObject.FindProperty("itemName");
        spSprite = serializedObject.FindProperty("itemSprite");
        spDescription = serializedObject.FindProperty("description");
        spItemId = serializedObject.FindProperty("itemId");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Texture2D texture2D = AssetPreview.GetAssetPreview(spSprite.objectReferenceValue);
        GUILayout.Box(texture2D);
        EditorGUILayout.LabelField("Name:", headerStyle);
        EditorGUILayout.LabelField(spName.stringValue, textStyle);
        EditorGUILayout.LabelField("Description:", headerStyle);
        EditorGUILayout.LabelField(spDescription.stringValue, textStyle);
        EditorGUILayout.LabelField("ItemID:", headerStyle);
        EditorGUILayout.LabelField(spItemId.stringValue, textStyle);
        serializedObject.ApplyModifiedProperties();
    }
}
