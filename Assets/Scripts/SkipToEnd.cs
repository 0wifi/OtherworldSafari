using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipToEnd : MonoBehaviour
{
    //called by inputaction
    public void OnSkipToEnd()
    {
        SceneManager.LoadScene("EndScene");
    }
}
