using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

#if UNITY_EDITOR
public class CameraCapture : MonoBehaviour
{
    [SerializeField] string _fileName;
    [SerializeField] int _captureWidth, _captureHeight;
    [SerializeField] int _fileCounter = 0;
    [SerializeField] Camera mainCamera;
    TextureCreationFlags flags;
    [SerializeField] bool _isTransparent;

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
        flags = TextureCreationFlags.DontInitializePixels;
        RenderTexture currentRT = new RenderTexture(_captureWidth, _captureHeight, 32, GraphicsFormat.B8G8R8A8_SRGB);
        currentRT.depthStencilFormat = GraphicsFormat.D24_UNorm_S8_UInt;

        mainCamera.targetTexture = currentRT;
        RenderTexture.active = currentRT;

        mainCamera.Render();

        Texture2D Image = new Texture2D(_captureWidth, _captureHeight, TextureFormat.RGBA32, false, true, true);

        Image.ReadPixels(new Rect(0, 0, _captureWidth, _captureHeight), 0, 0);
        Image.Apply();

        RenderTexture.active = null;
        mainCamera.targetTexture = null;

        if (_isTransparent)
        {
            Color32[] colors = Image.GetPixels32();
            for (int i = 0; i < colors.Length; i++)
            {
                //Debug.Log(colors[i].g);
                if (colors[i].r > 0 || colors[i].g > 0 || colors[i].b > 0)
                {
                    colors[i].a = 255;
                }
                else
                {
                    colors[i].a = 0;
                }
            }
            Image.SetPixels32(colors);
            Image.Apply();
        }

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
