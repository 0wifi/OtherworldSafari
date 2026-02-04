using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class AccuracyController : MonoBehaviour
{
    [SerializeField] private List<AccuracyRing> AccuracyRings;

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
    [Range(0,1)]
    public float pointValuePercentage;
}

