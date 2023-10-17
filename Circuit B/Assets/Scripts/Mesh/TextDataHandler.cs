using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;
using UnityEditor;

public class TextDataHandler : MonoBehaviour
{
    [SerializeField] SO_TextData textData;
    [SerializeField] List<string> values = new List<string>();
    [SerializeField] List<Vector3> points = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        //Create the XmlParserContext.
        XmlParserContext context = new XmlParserContext(null, null, "", XmlSpace.None);

        XmlTextReader reader = new XmlTextReader(textData.textAsset.text, XmlNodeType.Element, context);

        reader.MoveToContent();
        reader.ReadToDescendant("path");
        reader.MoveToAttribute("d");

        Debug.Log(reader.Value);

        values = reader.Value.Split(" ", System.StringSplitOptions.RemoveEmptyEntries).ToList();

        for (int i = 0; i < values.Count; i ++)
        {
            if (values[i] == "M")
            {
                points.Add(new Vector2(float.Parse(values[i + 1]), float.Parse(values[i + 2])));
            }
            else if (values[i] == "L")
            {
                points.Add(new Vector2(float.Parse(values[i + 1]), float.Parse(values[i + 2])));
            }
            else if (values[i] == "Z")
            {
                break;
            }
        }

        Debug.Log(points.Count);

        points.RemoveAt(points.Count - 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLineList(points.ToArray());
    }
}

public static class Util
{
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static T GetItem<T>(List<T> list, int index)
    {
        if (index >= list.Count)
        {
            return list[index % list.Count];
        }
        else if (index < 0)
        {
            return list[index % list.Count + list.Count];
        }
        else
        {
            return list[index];
        }
    }

    public static T GetItem<T>(T[] array, int index)
    {
        if (index >= array.Length)
        {
            return array[index % array.Length];
        }
        else if (index < 0) 
        { 
            return array[index % array.Length + array.Length];
        }
        else
        {
            return array[index];
        }
    }
}
