using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;
using UnityEditor;
#if UNITY_EDITOR
public class TextDataHandler : MonoBehaviour
{
    [SerializeField] SO_TextData textData;
    [SerializeField] List<string> values = new List<string>();
    [SerializeField] List<WordCharacters> _wordCharacters = new List<WordCharacters>();
    [SerializeField] bool _generateWordChar = false;
    [SerializeField] bool _generateMesh = false;
    [SerializeField] bool _createMesh = false;

    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] MeshRenderer _meshRenderer;

    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        if (_generateWordChar)
        {
            GetWordChar();
        }

        if (_generateMesh)
        {
            GenerateMesh();
        }

        if (_createMesh)
        {
            CreateMesh();
        }
    }

    void GetWordChar()
    {
        //Create the XmlParserContext.
        XmlParserContext context = new XmlParserContext(null, null, "", XmlSpace.None);

        XmlTextReader reader = new XmlTextReader(textData.textAsset.text, XmlNodeType.Element, context);

        reader.MoveToContent();
        reader.ReadToDescendant("path");
        reader.MoveToAttribute("d");

        Debug.Log(reader.Value);

        values = reader.Value.Split(" ", System.StringSplitOptions.RemoveEmptyEntries).ToList();

        int currentChar = 0;
        int currentPoint = 0;

        var iconContent = EditorGUIUtility.IconContent("sv_label_1");

        _wordCharacters.Add(new WordCharacters());

        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] == "M" || values[i] == "L")
            {
                _wordCharacters[currentChar].points.Add(new Vector3(float.Parse(values[i + 1]), float.Parse(values[i + 2]) * -1));
                GameObject temp = new GameObject(_wordCharacters[currentChar].points[currentPoint].ToString());
                temp.transform.position = _wordCharacters[currentChar].points[currentPoint];
                EditorGUIUtility.SetIconForObject(temp, (Texture2D)iconContent.image);

                Debug.Log($"Current Point {currentPoint}");
                currentPoint++;
            }
            else if (values[i] == "Z")
            {
                Debug.Log($"End of Character {currentChar}");
                currentChar++;
                currentPoint = 0;
                _wordCharacters.Add(new WordCharacters());
            }
        }
    }

    void GenerateMesh()
    {
        foreach (WordCharacters wordchar in _wordCharacters)
        {
            if (!PolygonHelper.Triangulate(wordchar.points.ToArray(), out triangles, out string errorMessage))
            {
                throw new System.Exception(errorMessage);
            }
        }
    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = _wordCharacters[0].points.ToArray();
        mesh.triangles = triangles;

        _meshFilter.mesh = mesh;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        int currentWord = 0;
        Gizmos.color = Color.black;

        foreach (WordCharacters wordchar in _wordCharacters)
        {
            for (int i = 0; i < wordchar.points.Count; i++)
            {
                if (i == wordchar.points.Count - 1)
                {
                    Gizmos.DrawLine(wordchar.points[i], wordchar.points[0]);
                }
                else
                {
                    Gizmos.DrawLine(this._wordCharacters[currentWord].points[i], this._wordCharacters[currentWord].points[i + 1]);
                }
            }
            currentWord++;
        }
    }
}

[System.Serializable]
public class WordCharacters
{
    public List<Vector3> points = new List<Vector3>();
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
#endif