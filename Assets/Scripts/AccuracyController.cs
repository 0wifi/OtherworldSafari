using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using NaughtyAttributes;

public class AccuracyController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private Transform testPos;

    [SerializeField] private List<AccuracyRing> AccuracyRings;

    private AnimationCurve curve;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (var accuracyRing in AccuracyRings)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            // Draw at rect's center, which will be offset from 0,0 based on the rect's position
            Gizmos.DrawWireCube(accuracyRing.rect.center, new Vector3(accuracyRing.rect.width, accuracyRing.rect.height, 0.1f));
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

    /// <summary>
    ///     Returns point value for the highest-value accuracy ring that contains the given position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public float GetValuePercentage(Vector2 position)
    {
        //maximum point value found
        float max = 0f;
        //convert world pos to local pos
        Vector2 localPos = transform.InverseTransformPoint(position);

        foreach (var accuracyRing in AccuracyRings)
        {
            if (accuracyRing.rect.Contains(localPos))
            {
                if (accuracyRing.pointValuePercentage > max)
                {
                    max = accuracyRing.pointValuePercentage;
                }
            }
        }
        return max;
    }

    [Button]
    private void DebugCenterRectangles()
    {
        foreach(var a in AccuracyRings)
        {
            a.rect.position = new Vector2(-a.rect.width / 2, -a.rect.height / 2);
        }
    }

    [Button]
    private void DebugAccuracyTest()
    {
        print(GetValuePercentage(testPos.position));
    }
}

[Serializable]
public class AccuracyRing
{
    public Rect rect;
    [Range(0,1)]
    public float pointValuePercentage;
}

