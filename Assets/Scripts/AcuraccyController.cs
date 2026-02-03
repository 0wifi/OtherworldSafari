using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class AcuraccyController : MonoBehaviour
{
    [SerializeField] private List<AccuracyRing> AccuracyRings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (var accuracyRing in AccuracyRings)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + accuracyRing.rect.position, transform.localToWorldMatrix * new Vector3(accuracyRing.rect.width, accuracyRing.rect.height, 0.1f));
        }
    }
}

[Serializable]
public class AccuracyRing
{
    public Rect rect;
    public float pointValuePercentage;
}

