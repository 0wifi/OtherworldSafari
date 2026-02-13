using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour
{
    [SerializeField] private GameObject captureImagePrefab;

    private Camera sceneCamera;


    private void Start()
    {
        TryGetComponent<Camera>(out sceneCamera);
    }

    public void CaptureImage(Camera camera)
    {
        //create new image instance
        GameObject imageInstance = Instantiate(captureImagePrefab, FindFirstObjectByType<Canvas>().transform);
        Sprite s = CaptureCamera(camera);
        imageInstance.GetComponent<Image>().sprite = s;
    }

    public Sprite CaptureCamera(Camera cam)
    {
        // temporary RenderTexture, assigned to the camera
        RenderTexture rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
        cam.targetTexture = rt;

        // create a Texture2D to hold pixel data
        Texture2D screenShot = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGBA32, false);

        // render the camera's view into the RenderTexture
        cam.Render();

        // read the pixels from the active RenderTexture into the Texture2D
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
        screenShot.Apply();

        // reset camera and active RenderTexture
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // create and return sprite
        return Sprite.Create(screenShot, new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), new Vector2(0.5f, 0.5f));
    }
}
