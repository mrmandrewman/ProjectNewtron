using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class ActorEnemyPath : MonoBehaviour
{
	[SerializeField]
	public PathPoint[] pathingPoints;

	public float GetPathDuration()
	{
		float duration = 0;
		for (int i = 0; i < pathingPoints.Length; i++)
		{
			duration += pathingPoints[i].GetDelayTime();
			duration += pathingPoints[i].GetTravelTime();
		}
		return duration;
	}

	// Get point from the progress time passed by enemy.
	public Vector3 GetPathPoint(float t, out bool IsFinished)
	{

		for (int i = 0; i < pathingPoints.Length - 1; i++)
		{
			// Subtract delay time from progress
			t -= pathingPoints[i].GetDelayTime();
			// If the time elapsed goes to the next bezier curve
			if (t - pathingPoints[i].GetTravelTime() > 0)
			{
				// remove the travel time of the curve just travelled
				t -= pathingPoints[i].GetTravelTime();
			}
			else
			{
				if (t <= 0)
				{
					// If time is less than zero make zero so it stays at start point of the curve, this adds the delay until it expends the delay time
					t = 0;
				}
				// Make time between 0 and 1 for the bezier function
				t = t / pathingPoints[i].GetTravelTime();
				// return the point
				IsFinished = false;
				return Bezier.GetPoint(transform.TransformPoint(pathingPoints[i].GetLocation()), transform.TransformPoint(pathingPoints[i].GetLocation() + pathingPoints[i].GetControlPoint(1)), transform.TransformPoint(pathingPoints[i + 1].GetLocation() + pathingPoints[i + 1].GetControlPoint(0)), transform.TransformPoint(pathingPoints[i + 1].GetLocation()), t);
			}
		}

		// return the final location possible
		IsFinished = true;
		return Bezier.GetPoint(transform.TransformPoint(pathingPoints[pathingPoints.Length - 2].GetLocation()), transform.TransformPoint(pathingPoints[pathingPoints.Length - 2].GetLocation() + pathingPoints[pathingPoints.Length - 2].GetControlPoint(1)), transform.TransformPoint(pathingPoints[pathingPoints.Length - 1].GetLocation() + pathingPoints[pathingPoints.Length - 1].GetControlPoint(0)), transform.TransformPoint(pathingPoints[pathingPoints.Length - 1].GetLocation()), 1);
	}

	public Vector3 GetPointLocation(int i)
	{
		return pathingPoints[i].GetLocation();
	}

	public void SetPointLocation(int i, Vector3 _location)
	{
		pathingPoints[i].SetLocation(_location);
	}

	public Vector3 GetControlPoint(int i, int j)
	{
		return pathingPoints[i].GetControlPoint(j);
	}

	public void SetControlPoint(int i, int j, Vector3 point)
	{
		// i refers to the point in the bezier spline, j refers to the control point for that spline point
		pathingPoints[i].SetControlPoint(j, point);
		EnforceMode(i, j);
	}

	public BezierControlPointMode GetPointMode(int i)
	{
		return pathingPoints[i].GetControlPointMode();
	}

	public void SetPointMode(int i, BezierControlPointMode _mode)
	{
		pathingPoints[i].SetControlPointMode(_mode);
	}

	public float GetDelayTime(int i)
	{
		return pathingPoints[i].GetDelayTime();
	}

	public void SetDelayTime(int i, float t)
	{
		pathingPoints[i].SetDelayTime(t);
	}

	public float GetTravelTime(int i)
	{
		return pathingPoints[i].GetTravelTime();
	}

	public void SetTravelTime(int i, float t)
	{
		pathingPoints[i].SetTravelTIme(t);
	}


	public void EnforceMode(int pointIndex, int fixedIndex)
	{
		BezierControlPointMode pointMode = pathingPoints[pointIndex].GetControlPointMode();
		if (pointMode == BezierControlPointMode.Free)
		{
			return;
		}

		// index of control point to be enforced/manipulated
		int enforcedIndex;
		if (fixedIndex == 0)
		{
			enforcedIndex = 1;
		}
		else
		{
			enforcedIndex = 0;
		}

		Vector3 enforcedTangent = Vector3.zero - pathingPoints[pointIndex].GetControlPoint(fixedIndex);

		if (pointMode == BezierControlPointMode.Aligned)
		{
			enforcedTangent = enforcedTangent.normalized * pathingPoints[pointIndex].GetControlPoint(enforcedIndex).magnitude;
		}
		pathingPoints[pointIndex].SetControlPoint(enforcedIndex, enforcedTangent);
	}

	public void AddCurve()
	{
		Vector3 previousPointLocaltion = pathingPoints[pathingPoints.Length - 1].GetLocation();
		Array.Resize(ref pathingPoints, pathingPoints.Length + 1);
		pathingPoints[pathingPoints.Length - 1] = new PathPoint(previousPointLocaltion + new Vector3(0, -4, 0));
		EnforceMode(pathingPoints.Length - 1, 0);
	}

	public void RemoveCurve()
	{
		if (pathingPoints.Length > 2)
		{
			Array.Resize(ref pathingPoints, pathingPoints.Length - 1);
		}
	}

	public void Reset()
	{
		pathingPoints = new PathPoint[]
		{
			new PathPoint(),
			new PathPoint(new Vector3(0,-4,0))
		};
	}
}
