using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour
{
    [SerializeField] private GameObject captureImagePrefab;

    public Texture2D outputTexture;

    public static List<Sprite> SavedSprites = new List<Sprite>();

    public IEnumerator CaptureImage(Camera camera)
    {
        yield return new WaitForEndOfFrame();

        //wait for sprite creation
        Sprite s = CaptureCameraAsync(camera);

        //create new image instance
        GameObject imageInstance = Instantiate(captureImagePrefab, FindFirstObjectByType<Canvas>().transform);
        imageInstance.transform.GetChild(0).GetComponent<Image>().sprite = s;
        imageInstance.GetComponent<RectTransform>().sizeDelta = new Vector2((s.rect.width / s.rect.height) * imageInstance.GetComponent<RectTransform>().rect.height, imageInstance.GetComponent<RectTransform>().rect.height);
        imageInstance.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10.0f, 10.0f));
    }
    public IEnumerator CaptureImageAndSaveSprite(Camera camera)
    {
        yield return new WaitForEndOfFrame();

        //wait for sprite creation
        Sprite s = CaptureCameraAsync(camera);

        //create new image instance
        GameObject imageInstance = Instantiate(captureImagePrefab, FindFirstObjectByType<Canvas>().transform);
        imageInstance.transform.GetChild(0).GetComponent<Image>().sprite = s;
        imageInstance.GetComponent<RectTransform>().sizeDelta = new Vector2((s.rect.width / s.rect.height) * imageInstance.GetComponent<RectTransform>().rect.height, imageInstance.GetComponent<RectTransform>().rect.height);

        //Maintain sprite for use in end screen
        SavedSprites.Add(s);
    }

    public Sprite CaptureCameraAsync(Camera cam)
    {
        OutputTextureFromCamera(cam);
        // Create a sprite from the outputTexture after the asynchronous capture is complete
        return Sprite.Create(outputTexture, new Rect(0, 0, outputTexture.width, outputTexture.height), new Vector2(0.5f, 0.5f));
    }

    private void OutputTextureFromCamera(Camera cam)
    {
        RenderTexture rt = new RenderTexture((int)(cam.pixelWidth * 0.8), cam.pixelHeight, 24);
        //print($"{cam.pixelWidth}, {cam.pixelHeight}");
        cam.targetTexture = rt;

        // render the camera into the RenderTexture
        cam.Render();

        RenderTexture lowResTexture = RenderTexture.GetTemporary(rt.width / 4, rt.height / 4, 0);

        // downsample
        Graphics.Blit(rt, lowResTexture);

        // initialize outputTexture
        outputTexture = new Texture2D(lowResTexture.width, lowResTexture.height, TextureFormat.RGBA32, false);

        // reset camera target
        cam.targetTexture = null;

        AsyncGPUReadback.Request(lowResTexture, 0, TextureFormat.RGBA32, (request) =>
        {
            if (request.hasError)
            {
                Debug.LogError("GPU readback error detected.");
            }
            else
            {
                outputTexture.LoadRawTextureData(request.GetData<byte>());
                outputTexture.Apply();
            }
        });
    }
}
