using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour
{
    [SerializeField] private GameObject captureImagePrefab;

    public Texture2D outputTexture;

    public IEnumerator CaptureImage(Camera camera)
    {
        yield return new WaitForEndOfFrame();

        //wait for sprite creation
        Sprite s = CaptureCameraAsync(camera);

        //create new image instance
        GameObject imageInstance = Instantiate(captureImagePrefab, FindFirstObjectByType<Canvas>().transform);
        imageInstance.GetComponent<Image>().sprite = s;
    }

    public Sprite CaptureCameraAsync(Camera cam)
    {
        OutputTextureFromCamera(cam);
        // Create a sprite from the outputTexture after the asynchronous capture is complete
        return Sprite.Create(outputTexture, new Rect(0, 0, outputTexture.width, outputTexture.height), new Vector2(0.5f, 0.5f));
    }

    private void OutputTextureFromCamera(Camera cam)
    {
        RenderTexture rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
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
