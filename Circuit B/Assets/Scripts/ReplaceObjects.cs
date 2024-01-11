using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class ReplaceObjects : MonoBehaviour
{
    public List<GameObject> replaceObject = new List<GameObject>();
    public GameObject parent;

    public void Replace()
    {
        List<GameObject> toReplace = new List<GameObject>();
        toReplace.AddRange(GameObject.FindGameObjectsWithTag("Replace"));

        foreach (GameObject old in toReplace)
        {
            foreach(GameObject newObj in replaceObject)
            {
                if (old.name.Contains(newObj.name))
                {
                    Vector3 pos = old.transform.position;
                    Quaternion quaternion = old.transform.rotation;

                    GameObject replacement = (GameObject)PrefabUtility.InstantiatePrefab(newObj, parent.transform);
                    replacement.transform.position = pos;
                    replacement.transform.rotation = quaternion;

                    old.SetActive(false);
                }
            }
        }
    }
}

[CustomEditor(typeof(ReplaceObjects))]
public class ReplaceObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ReplaceObjects _target = (ReplaceObjects)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Replace"))
        {
            _target.Replace();
        }
    }
}
#endif
