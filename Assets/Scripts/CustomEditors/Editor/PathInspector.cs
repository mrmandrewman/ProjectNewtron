using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActorEnemyPath))]
public class PathInspector : Editor
{
	private ActorEnemyPath path;

	private Transform handleTransform;
	private Quaternion handleRotation;

	private const float handleSize = 0.04f;
	private const float pickSize = 0.06f;
	private static Color[] modeColours =
	{
		Color.green,
		Color.yellow,
		Color.cyan
	};


	private Vector2Int selectedIndex = new Vector2Int(-1, -1);

	private bool[] expandItem = { false };
	private bool[] usePolarCoord = { false };

	private void OnSceneGUI()
	{
		path = target as ActorEnemyPath;

		handleTransform = path.transform;

		if (Tools.pivotRotation == PivotRotation.Local)
		{
			handleRotation = handleTransform.rotation;
		}
		else
		{
			handleRotation = Quaternion.identity;
		}

		for (int i = 0; i < path.pathingPoints.Length; i++)
		{
			ShowPoint(i);

			if (i < path.pathingPoints.Length - 1)
			{
				Handles.DrawBezier(handleTransform.TransformPoint(path.GetPointLocation(i)), handleTransform.TransformPoint(path.GetPointLocation(i + 1)), handleTransform.TransformPoint(path.GetControlPoint(i, 1) + path.GetPointLocation(i)), handleTransform.TransformPoint(path.GetControlPoint(i + 1, 0) + path.GetPointLocation(i + 1)), Color.white, null, 2f);
			}
		}

	}

	private void ShowPoint(int index)
	{
		// Show the point that the path passes through
		Vector3 point = path.GetPointLocation(index);

		// Store the control point location in variables
		Vector3 controlPoint0Location = path.GetControlPoint(index, 0) + point;
		Vector3 controlPoint1Location = path.GetControlPoint(index, 1) + point;

		// Draw lines between the mid point and the control points to show what control points are connected to the path point
		Handles.color = Color.grey;
		Handles.DrawLine(handleTransform.TransformPoint(controlPoint0Location), handleTransform.TransformPoint(point));
		Handles.DrawLine(handleTransform.TransformPoint(point), handleTransform.TransformPoint(controlPoint1Location));

		// Store what point is selected so that a Handle can be drawn on that point
		Handles.color = Color.white;
		if (Handles.Button(handleTransform.TransformPoint(point), handleRotation, handleSize, pickSize, Handles.DotHandleCap))
		{
			selectedIndex = new Vector2Int(index, -1);
			Repaint();
		}

		// If the selected point is the path point being accessed in the for loop
		if (selectedIndex[0] == index && selectedIndex[1] == -1)
		{
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(handleTransform.TransformPoint(point), handleRotation);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(path, "Move Point");
				path.SetPointLocation(index, handleTransform.InverseTransformPoint(point));
				EditorUtility.SetDirty(path);
			}
		}

		// Set the control point colour based on the mode
		Handles.color = modeColours[(int)path.GetPointMode(index)];

		// Show the controlPoint 0 handle if selected
		if (Handles.Button(handleTransform.TransformPoint(controlPoint0Location), handleRotation, handleSize, pickSize, Handles.DotHandleCap))
		{
			selectedIndex = new Vector2Int(index, 0);
			Repaint();
		}
		// If control point has been changed
		if (selectedIndex[0] == index && selectedIndex[1] == 0)
		{
			EditorGUI.BeginChangeCheck();
			controlPoint0Location = Handles.DoPositionHandle(handleTransform.TransformPoint(controlPoint0Location), handleRotation);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(path, "Move Control Point");
				path.SetControlPoint(index, 0, handleTransform.InverseTransformPoint(controlPoint0Location - point));
				EditorUtility.SetDirty(path);
			}
		}

		// Show the controlPoint 1 and set it if selected and changed
		if (Handles.Button(handleTransform.TransformPoint(controlPoint1Location), handleRotation, handleSize, pickSize, Handles.DotHandleCap))
		{
			selectedIndex = new Vector2Int(index, 1);
			Repaint();
		}
		if (selectedIndex[0] == index && selectedIndex[1] == 1)
		{
			EditorGUI.BeginChangeCheck();
			controlPoint1Location = Handles.DoPositionHandle(handleTransform.TransformPoint(controlPoint1Location), handleRotation);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(path, "Move Control Point");
				path.SetControlPoint(index, 1, handleTransform.InverseTransformPoint(controlPoint1Location - point));
				EditorUtility.SetDirty(path);
			}
		}

	}


	public override void OnInspectorGUI()
	{

		path = target as ActorEnemyPath;

		// Resize arrays expandItem and usePolarCoord to match the amount of points
		if (expandItem.Length != path.pathingPoints.Length)
		{
			int oldSize = expandItem.Length;
			Array.Resize(ref expandItem, path.pathingPoints.Length);
			Array.Resize(ref usePolarCoord, path.pathingPoints.Length);
			for (int i = oldSize - 1; i < expandItem.Length; i++)
			{
				expandItem[i] = false;
				usePolarCoord[i] = false;
			}
		}
		


		// Use for tooltip for point timings in point foldout
		float ElaspedTravelTime = 0.0f;

		EditorGUILayout.LabelField("Enemy Path");
		for (int i = 0; i < path.pathingPoints.Length; i++)
		{

			// The foldout for the currently selected point will be bold
			if (i == selectedIndex.x)
			{
				GUIStyle style = EditorStyles.foldout;
				FontStyle previousStyle = style.fontStyle;
				style.fontStyle = FontStyle.Bold;
				expandItem[i] = EditorGUILayout.Foldout(expandItem[i], "Point " + i + "\tArrival Time: " + ElaspedTravelTime + "\tDepature Time: " + (ElaspedTravelTime + path.pathingPoints[i].GetDelayTime()), style);
				style.fontStyle = previousStyle;
			}
			else
			{
				expandItem[i] = EditorGUILayout.Foldout(expandItem[i], "Point " + i + "\tArrival Time: " + ElaspedTravelTime + "\tDepatureTime: " + (ElaspedTravelTime + path.pathingPoints[i].GetDelayTime()));
			}

			// Add the total time the point will take for the next points timing tooltip
			ElaspedTravelTime += path.pathingPoints[i].GetDelayTime() + path.pathingPoints[i].GetTravelTime();

			if (expandItem[i])
			{
				EditorGUI.indentLevel++;
				EditorGUI.BeginChangeCheck();

				Vector3 point = EditorGUILayout.Vector2Field("Location", path.GetPointLocation(i));
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(path, "Move Point");
					path.SetPointLocation(i, point);
					EditorUtility.SetDirty(path);
				}

				// Inspector Control Points
				EditorGUILayout.LabelField("Control Points", EditorStyles.boldLabel);

				// Toggle to display control points as Cartesian Coordinates(Default) or Polar Coordinates
				usePolarCoord[i] = EditorGUILayout.Toggle("As Polar Coordinates", usePolarCoord[i]);

				EditorGUI.indentLevel++;
				EditorGUI.BeginChangeCheck();
				Vector3 controlPoint0;
				// If using polar coordinates convert to polar coordinates
				if (usePolarCoord[i])
				{
					controlPoint0 = EditorGUILayout.Vector2Field("Control Point 0", CartesianToPolar( path.GetControlPoint(i, 0)));
				}
				else // Carry on using Cartesian as normal
				{
					controlPoint0 = EditorGUILayout.Vector2Field("Control Point 0", path.GetControlPoint(i, 0));
				}
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(path, "Move Point");
					// If using Polar Coordinates convert back to Cartesian
					if (usePolarCoord[i])
					{
						controlPoint0 = PolarToCartesian(controlPoint0);
					}
					path.SetControlPoint(i, 0, controlPoint0);
					EditorUtility.SetDirty(path);
				}

				// Second Control Point
				EditorGUI.BeginChangeCheck();
				Vector3 controlPoint1;
				if (usePolarCoord[i])
				{
					controlPoint1 = EditorGUILayout.Vector2Field("Control Point 1", CartesianToPolar(path.GetControlPoint(i, 1)));
				}
				else
				{
					controlPoint1 = EditorGUILayout.Vector2Field("Control Point 1", path.GetControlPoint(i, 1));
				}
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(path, "Move Point");
					if (usePolarCoord[i])
					{
						controlPoint1 = PolarToCartesian(controlPoint1);
					}
					path.SetControlPoint(i, 1, controlPoint1);
					EditorUtility.SetDirty(path);
				}
				EditorGUI.indentLevel--;


				// Inspector Control Point Mode
				EditorGUI.BeginChangeCheck();
				BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", path.GetPointMode(i));
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(path, "Change Point Mode");
					path.SetPointMode(i, mode);
					path.EnforceMode(i, 0);
					EditorUtility.SetDirty(path);
				}
				
				// Travel Time
				EditorGUI.BeginChangeCheck();
				float travelTime = EditorGUILayout.FloatField("Travel Time", path.GetTravelTime(i));
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(path, "Change Travel Time");
					path.SetTravelTime(i, travelTime);
					EditorUtility.SetDirty(path);
				}

				// Delay Time
				EditorGUI.BeginChangeCheck();
				float delayTime = EditorGUILayout.FloatField("Delay Time", path.GetDelayTime(i));
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(path, "Change Travel Time");
					path.SetDelayTime(i, delayTime);
					EditorUtility.SetDirty(path);

				}
			}
			// Reset indent Level
			EditorGUI.indentLevel = 0;

		}

		if (GUILayout.Button("Add Point"))
		{
			Undo.RecordObject(path, "Add Point");
			path.AddCurve();
			EditorUtility.SetDirty(path);
		}

		if (path.pathingPoints.Length > 2)
		{
			if (GUILayout.Button("Remove Point"))
			{
				Undo.RecordObject(path, "Remove Point");
				path.RemoveCurve();
				EditorUtility.SetDirty(path);
			}
		}

	}

	Vector2 CartesianToPolar(Vector2 Cartesian)
	{
		Vector2 Polar;
		// Polar.y is the angle of the polar coordinates
		Polar.y = Mathf.Atan2(Cartesian.y, Cartesian.x) * Mathf.Rad2Deg;
		// Polar.x is the magnitude
		if (Cartesian.magnitude <= 0)
		{
			Polar.x = 0;
		}
		else
		{
			Polar.x = Cartesian.magnitude;
		}
	
		return Polar;
	}

	Vector2 PolarToCartesian(Vector2 Polar)
	{
		Vector2 Cartesian;
		Cartesian.x = Polar.x * Mathf.Cos(Polar.y*Mathf.Deg2Rad);
		Cartesian.y = Polar.x * Mathf.Sin(Polar.y * Mathf.Deg2Rad);
		return Cartesian;
	}
}