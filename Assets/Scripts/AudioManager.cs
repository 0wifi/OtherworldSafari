using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [MinMaxSlider(0.5f, 1.5f)]
    public Vector2 minMaxRandomize = new Vector2(0.9f, 1.1f);

    [SerializeField] public GameObject SoundEffectInstance;

    [SerializeField] public List<GameObject> CameraHit;
    [SerializeField] public GameObject CameraMiss;
    
    public void InstantiateRandomOfList(List<GameObject> list, Transform point, bool randomizePitch)
    {
        InstantiateSound(list[Random.Range(0, list.Count - 1)], point, randomizePitch);
    }
    public void InstantiateSound(GameObject soundEffectIsntance, Transform point, bool randomizePitch)
    {
        GameObject instance = Instantiate(soundEffectIsntance, point.position, Quaternion.identity);

        AudioSource audioSource = instance.GetComponent<AudioSource>();
        audioSource.pitch = randomizePitch ? Random.Range(minMaxRandomize.x, minMaxRandomize.y) : audioSource.pitch;

        SoundEffect soundEffect = instance.GetComponent<SoundEffect>();
        soundEffect.StartCoroutine(soundEffect.DestroyAfterTime(audioSource.clip.length));
    }

    public void PlayRandomOfList(List<AudioClip> list, Transform point, bool randomizePitch)
    {
        PlaySound(list[Random.Range(0, list.Count - 1)], point, randomizePitch);
    }

    public void PlaySound(AudioClip clip, Transform point, bool randomizePitch)
    {
        GameObject instance = Instantiate(SoundEffectInstance, point.position, Quaternion.identity);

        AudioSource audioSource = instance.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = randomizePitch ? Random.Range(minMaxRandomize.x,minMaxRandomize.y) : audioSource.pitch;
        audioSource.Play();

        SoundEffect soundEffect = instance.GetComponent<SoundEffect>();
        soundEffect.StartCoroutine(soundEffect.DestroyAfterTime(audioSource.clip.length));
    }
}
