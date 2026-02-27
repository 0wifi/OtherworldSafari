using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;

public class SplineSpeedAdjust : MonoBehaviour
{
    public SplineContainer container;
    public Spline spline;
    public SplineAnimate path;
    public int currentKnot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spline = container.Spline;
    }

    // Update is called once per frame
    void Update()
    {
        float knotIndexFloat = SplineUtility.ConvertIndexUnit(spline, path.NormalizedTime, PathIndexUnit.Normalized, 
            PathIndexUnit.Knot);
        currentKnot = Mathf.FloorToInt(knotIndexFloat);
        if(currentKnot > 2)
        {
            path.MaxSpeed = 4;
        }
        print(knotIndexFloat);
    }
}
