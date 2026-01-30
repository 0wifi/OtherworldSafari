using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;
using System.Linq;

public class BackgroundParallaxController : MonoBehaviour
{
    [SerializeField] private List<GameObject> BackgroundObjects;
    [SerializeField] private List<float> BackgroundSpeeds;

    private List<GameObject> _backgroundDuplicates;
    private float[] _backgroundWidths;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _backgroundWidths = new float[BackgroundObjects.Count];
        _backgroundDuplicates = new List<GameObject>();

        for (int i = 0; i < BackgroundObjects.Count; i++ )
        {
            //Get widths of background sprites
            SpriteRenderer sr;
            if (BackgroundObjects[i].TryGetComponent<SpriteRenderer>(out sr))
            {
                _backgroundWidths[i] = sr.bounds.size.x;
            } else throw new System.Exception("Background Object has no SpriteRenderer");

            //Duplicate background objects offscreen
            Vector2 offset = new Vector2(_backgroundWidths[i], 0);
            _backgroundDuplicates.Add(Instantiate(BackgroundObjects[i], ((Vector2)BackgroundObjects[i].transform.position + offset), Quaternion.identity));
        }

        if (BackgroundSpeeds.Count < BackgroundObjects.Count)
        {
            Debug.LogWarning("Count of BackgroundSpeeds is less than count of BackgroundObjects. Process will use default value.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < BackgroundObjects.Count; i++)
        {
            MoveAndRepositionBackground(i);
        }
    }

    private void MoveAndRepositionBackground(int idx)
    {
        if (BackgroundObjects[idx] != null && _backgroundDuplicates[idx] != null)
        {
            float speed;
            try { speed = BackgroundSpeeds[idx]; }
            catch (System.Exception)
            {
                speed = 5;
            }
            //translate objects by speed
            BackgroundObjects[idx].transform.Translate(Vector2.left * speed * Time.deltaTime);
            _backgroundDuplicates[idx].transform.Translate(Vector2.left * speed * Time.deltaTime);

            //if object goes offscreen, reposition by adding twice its width to x
            if (BackgroundObjects[idx].transform.position.x < -_backgroundWidths[idx])
            {
                Vector2 offset = new Vector2(_backgroundWidths[idx] * 2, 0);
                BackgroundObjects[idx].transform.position = (Vector2)BackgroundObjects[idx].transform.position + offset;
            }
            if (_backgroundDuplicates[idx].transform.position.x < -_backgroundWidths[idx])
            {
                Vector2 offset = new Vector2(_backgroundWidths[idx] * 2, 0);
                _backgroundDuplicates[idx].transform.position = (Vector2)_backgroundDuplicates[idx].transform.position + offset;
            }
        }
    }
}
