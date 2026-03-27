using UnityEngine;

public class MotorManager : MonoBehaviour
{
    [SerializeField] private AudioClip motorSFX;
    [SerializeField] private AudioSource motorSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void MotorSound()
    {
        motorSource.PlayOneShot(motorSFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}