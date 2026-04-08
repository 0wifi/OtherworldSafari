using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public enum Type
    {
        FadeToBlack,
        FadeToClear
    }
    public Type type;
    public Animation anim;
    private void Start()
    {
        switch (type)
        {
            case Type.FadeToBlack:
                GetComponent<Image>().color = new Color(0, 0, 0, 0);
                break;
            case Type.FadeToClear:
                GetComponent<Image>().color = new Color(0, 0, 0, 1);
                break;
            default: break;
        } 
    }

    public void StartFade()
    {
        anim.Play();
    }
}
