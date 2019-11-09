using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PathPoint {
	
	[SerializeField] Vector3 location;
	[SerializeField] Vector3[] controlPoints;
	[SerializeField] BezierControlPointMode mode;
	[SerializeField] float travelTime;
	[SerializeField] float delayTime;

	public PathPoint()
	{
		location = new Vector3(0.0f, 0.0f, 0.0f);
		controlPoints = new Vector3[]
		{
			new Vector3 (0,1,0),
			new Vector3 (0,-1,0)
		};
		mode = BezierControlPointMode.Mirrored;
		delayTime = 0.0f;
		travelTime = 1.0f;
	}

	public PathPoint(Vector3 _location)
	{
		location = _location;
		controlPoints = new Vector3[]
		{
			new Vector3 (0,1,0),
			new Vector3 (0,-1,0)
		};
		mode = BezierControlPointMode.Mirrored;
		delayTime = 0.0f;
		travelTime = 1.0f;
	}

	public Vector3 GetLocation()
	{
		return location;
	}

	public void SetLocation(Vector3 _location)
	{
		location = _location;
	}

	public Vector3 GetControlPoint(int i)
	{
		return controlPoints[i];
	}

	public void SetControlPoint(int i, Vector3 _point)
	{
		controlPoints[i] = _point;
	}

	public BezierControlPointMode GetControlPointMode()
	{
		return mode;
	}

	public void SetControlPointMode(BezierControlPointMode _mode)
	{
		mode = _mode;
	}

	public float GetDelayTime()
	{
		return delayTime;
	}

	public void SetDelayTime(float _time)
	{
		delayTime = _time;
	}

	public float GetTravelTime()
	{
		return travelTime;
	}

	public void SetTravelTIme(float _time)
	{
		travelTime = _time;
	}
}
