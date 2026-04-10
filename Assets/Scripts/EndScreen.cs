using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    private ScoreManager scoreManager;
    public TMP_Text scoreText;
    [SerializeField] private PlayerInput playerInput;
    private InputAction returnToStart;

    public GameObject ImagePrefab;
    [SerializeField] private GameObject galleryContainer;
    private GameObject galleryDuplicate;

    public float galleryScrollSpeed = 1.0f;

    public FadeToBlack fadeToBlack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        fadeToBlack.StartFade();

        playerInput.currentActionMap.Enable();

        returnToStart = playerInput.currentActionMap.FindAction("ReturnToStart");

        StartCoroutine(DelayActivateInput(4f));
    }

    public IEnumerator DelayActivateInput(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        returnToStart.started += ReturnToStart_started;
    }

    private void ReturnToStart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene("StartScene");
        ImageCapture.SavedSprites = new System.Collections.Generic.List<Sprite>();
        ScoreManager.score = 0;
    }

    void Awake()
    {
        //ButtonPressCountJSONService.SaveLastRunCounts();
        //ButtonPressCountJSONService.pressCounts = new ButtonPressCountData();

        if (scoreText != null) scoreText.text = "Score: " + ScoreManager.score.ToString("N0");

        foreach (var s in ImageCapture.SavedSprites)
        {
            GameObject instance = Instantiate(ImagePrefab, galleryContainer.transform);
            instance.transform.GetChild(0).GetComponent<Image>().sprite = s;
            instance.GetComponent<RectTransform>().sizeDelta = new Vector2((s.rect.width / s.rect.height) * instance.GetComponent<RectTransform>().rect.height, instance.GetComponent<RectTransform>().rect.height);
        }
        StartCoroutine(UpdateHLG());
    }

    private IEnumerator UpdateHLG()
    {
        yield return null;
        galleryContainer.GetComponent<HorizontalLayoutGroup>().enabled = false;
        galleryContainer.GetComponent<HorizontalLayoutGroup>().enabled = true;

        StartCoroutine(DuplicateGallery());
    }

    private IEnumerator DuplicateGallery()
    {
        galleryDuplicate = Instantiate(galleryContainer, FindFirstObjectByType<Canvas>().transform);
        galleryDuplicate.GetComponent<HorizontalLayoutGroup>().enabled = false;
        galleryDuplicate.GetComponent<HorizontalLayoutGroup>().enabled = true;
        yield return null;
        RectTransform rectTransform = galleryContainer.GetComponent<RectTransform>();
        //print(rectTransform.rect.width);
        galleryDuplicate.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectTransform.rect.x + rectTransform.rect.width, rectTransform.position.y);
        yield return null;

        StartCoroutine(ScrollGallery());
    }

    private IEnumerator ScrollGallery()
    {
        RectTransform galleryRectTransform = galleryContainer.GetComponent<RectTransform>();
        RectTransform duplicateRectTransform = galleryDuplicate.GetComponent<RectTransform>();
        while (true)
        {
            //Move galleries to left
            galleryRectTransform.anchoredPosition = new Vector2(galleryRectTransform.anchoredPosition.x - galleryScrollSpeed * Time.deltaTime, galleryRectTransform.anchoredPosition.y);
            duplicateRectTransform.anchoredPosition = new Vector2(duplicateRectTransform.anchoredPosition.x - galleryScrollSpeed * Time.deltaTime, duplicateRectTransform.anchoredPosition.y);

            //Move to right of screen after has passed to the left
            if (galleryRectTransform.anchoredPosition.x < -galleryRectTransform.rect.width)
            {
                galleryRectTransform.anchoredPosition = new Vector2(galleryRectTransform.rect.width, galleryRectTransform.anchoredPosition.y);
            }
            if (duplicateRectTransform.anchoredPosition.x < -duplicateRectTransform.rect.width)
            {
                duplicateRectTransform.anchoredPosition = new Vector2(duplicateRectTransform.rect.width, duplicateRectTransform.anchoredPosition.y);
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        returnToStart.started -= ReturnToStart_started;
    }
}