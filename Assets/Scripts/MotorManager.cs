using UnityEngine;

public class MotorManager : MonoBehaviour
{
    [SerializeField] private AudioClip motorSFX;
    [SerializeField] private AudioSource motorSource;

    private static MotorManager instance;   

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //prevent duplicated audio source
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        motorSource = GetComponent<AudioSource>();
    }
    public void MotorSound()
    {
        motorSource = instance?.GetComponent<AudioSource>();
        motorSource.PlayOneShot(motorSFX); 
    }
}