using UnityEngine;

public class MotorManager : MonoBehaviour
{
    [SerializeField] private AudioClip motorSFX;
    [SerializeField] private AudioSource motorSource;

    private static MotorManager instance;   

    private void Awake()
    {
        motorSource.Stop();

        //prevent duplicated audio source
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        motorSource = GetComponent<AudioSource>();
    }
    public void MotorSound()
    {
        motorSource = GetComponent<AudioSource>();
        motorSource.PlayOneShot(motorSFX); 
    }
}