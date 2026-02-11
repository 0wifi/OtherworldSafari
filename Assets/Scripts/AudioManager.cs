using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] public GameObject SoundEffectInstance;

    [SerializeField] public List<AudioClip> CameraSounds;
    [SerializeField] public AudioClip CameraMiss;

    
    public void PlayRandomOfList(List<AudioClip> list, Transform point, bool randomizePitch)
    {
        PlaySound(list[Random.Range(0, list.Count - 1)], point, randomizePitch);
    }

    public void PlaySound(AudioClip clip, Transform point, bool randomizePitch)
    {
        GameObject instance = Instantiate(SoundEffectInstance, point.position, Quaternion.identity);

        AudioSource audioSource = instance.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = randomizePitch ? Random.Range(0.9f,1.1f) : audioSource.pitch;
        audioSource.Play();

        SoundEffect soundEffect = instance.GetComponent<SoundEffect>();
        soundEffect.StartCoroutine(soundEffect.DestroyAfterTime(audioSource.clip.length));
    }
}
