using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour
{
    [SerializeField] private Image image;

    private Camera camera;


    private void Start()
    {
        TryGetComponent<Camera>(out camera);
    }

    [Button]
    private void DebugCapture()
    {
        Texture2D texture = GetTextureFromCamera(camera);
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100f);
        image.sprite = newSprite;
    }

    private static Texture2D GetTextureFromCamera(Camera mCamera)
    {
        Rect rect = new Rect(0, 0, mCamera.pixelWidth, mCamera.pixelHeight);
        RenderTexture renderTexture = new RenderTexture(mCamera.pixelWidth, mCamera.pixelHeight, 24);
        Texture2D screenShot = new Texture2D(mCamera.pixelWidth, mCamera.pixelHeight, TextureFormat.RGBA32, false);

        mCamera.targetTexture = renderTexture;
        mCamera.Render();

        RenderTexture.active = renderTexture;

        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();


        mCamera.targetTexture = null;
        RenderTexture.active = null;
        return screenShot;
    }
}
