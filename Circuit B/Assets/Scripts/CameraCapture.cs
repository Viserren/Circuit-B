using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

#if UNITY_EDITOR
public class CameraCapture : MonoBehaviour
{
    [SerializeField] string _fileName;
    [SerializeField] int _captureWidth, _captureHeight;
    [SerializeField] int _fileCounter = 0;
    [SerializeField] Camera mainCamera;
    TextureCreationFlags flags;

    private void Start()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();
        CamCapture();
    }

    public void CamCapture()
    {
        //mainCamera = Camera.main;
        //mainCamera.clearFlags = CameraClearFlags.Depth;

        RenderTexture currentRT = new RenderTexture(_captureWidth, _captureHeight, 32, GraphicsFormat.R8G8B8A8_UNorm);
        mainCamera.targetTexture = currentRT;
        RenderTexture.active = mainCamera.targetTexture;

        mainCamera.Render();

        Texture2D Image = new Texture2D(_captureWidth, _captureHeight, TextureFormat.ARGB32, false);
        Image.ReadPixels(new Rect(0, 0, _captureWidth, _captureHeight), 0, 0);
        Image.Apply();

        //RenderTexture.active = null;
        //mainCamera.targetTexture = null;

        var Bytes = Image.EncodeToPNG();
        DestroyImmediate(Image);

        File.WriteAllBytes($"{Application.dataPath}/Backgrounds/{_fileName} {_fileCounter}.png", Bytes);
        _fileCounter++;
    }
}

[CustomEditor(typeof(CameraCapture))]
public class CameraCaptureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraCapture _target = (CameraCapture)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Capture"))
        {
            _target.CamCapture();
        }
    }
}
#endif
