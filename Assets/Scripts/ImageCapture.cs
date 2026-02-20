using NaughtyAttributes;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour
{
    [SerializeField] private GameObject captureImagePrefab;

    public Texture2D outputTexture;

    public async Task CaptureImage(Camera camera)
    {
        //wait for sprite creation
        Sprite s = await CaptureCameraAsync(camera);

        //create new image instance
        GameObject imageInstance = Instantiate(captureImagePrefab, FindFirstObjectByType<Canvas>().transform);
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

    public async Awaitable<Sprite> CaptureCameraAsync(Camera cam)
    {
        await OutputTextureFromCamera(cam);
        // Create a sprite from the outputTexture after the asynchronous capture is complete
        return Sprite.Create(outputTexture, new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), new Vector2(0.5f, 0.5f));

    }

    private async Awaitable OutputTextureFromCamera(Camera cam)
    {
        RenderTexture rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
        cam.targetTexture = rt;

        // render the camera into the RenderTexture
        cam.Render();

        // initialize outputTexture
        outputTexture = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGBA32, false);

        // reset camera target
        cam.targetTexture = null;

        var tcs = new TaskCompletionSource<bool>();

        AsyncGPUReadback.Request(rt, 0, TextureFormat.RGBA32, (request) =>
        {
            if (request.hasError)
            {
                Debug.LogError("GPU readback error detected.");
                tcs.SetResult(false);
            }
            else
            {
                outputTexture.LoadRawTextureData(request.GetData<byte>());
                outputTexture.Apply();
                tcs.SetResult(true);
            }

            Destroy(rt);
        });

        await tcs.Task;
    }
}
