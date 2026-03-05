using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class SplineSpeedAdjust : MonoBehaviour
{
    public SplineContainer container;
    public Spline spline;
    public SplineAnimate path;
    private int currentKnot;
    public float startSpeed;
    public float newSpeed;
    public int knotAdjustSpeed;
    private bool stupidCheck = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spline = container.Spline;
        path.MaxSpeed = startSpeed;
        StartCoroutine(ChangeSplineSpeed());
    }

    IEnumerator ChangeSplineSpeed()
    {
        float knotIndexFloat = SplineUtility.ConvertIndexUnit(spline, path.NormalizedTime, PathIndexUnit.Normalized,
            PathIndexUnit.Knot);
        currentKnot = Mathf.FloorToInt(knotIndexFloat);
        print(currentKnot);
        if (currentKnot > knotAdjustSpeed)
        {
            path.MaxSpeed = newSpeed;
            stupidCheck = true;
        }
        yield return new WaitForSeconds(0.1f);
        if(!stupidCheck)
        {
            StartCoroutine(ChangeSplineSpeed());
        }
    }
}
