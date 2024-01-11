using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class CameraCapture : MonoBehaviour
{
    public int FileCounter = 0;
    [SerializeField] Camera mainCamera;

    public void CamCapture()
    {
        //mainCamera = Camera.main;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = mainCamera.targetTexture;

        mainCamera.Render();

        Texture2D Image = new Texture2D(mainCamera.targetTexture.width, mainCamera.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, mainCamera.targetTexture.width, mainCamera.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        DestroyImmediate(Image);

        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + FileCounter + ".png", Bytes);
        FileCounter++;
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
