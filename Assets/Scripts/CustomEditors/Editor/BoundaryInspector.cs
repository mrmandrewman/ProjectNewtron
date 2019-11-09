using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActorBounds))]
public class BoundsInspector : Editor
{
	private ActorBounds bounds;

	private Transform handleTransform;
	private Quaternion handleRotation;

	private const float handleSize = 0.04f;

	private void OnSceneGUI()
	{
		bounds = target as ActorBounds;

		Handles.DrawSolidRectangleWithOutline(new Rect(bounds.playerBoundsPosition - bounds.playerBoundsSize /2, bounds.playerBoundsSize), new Color(0, 0, 0, 0), Color.green);
		Handles.DrawSolidRectangleWithOutline(new Rect(bounds.cameraBoundsPosition - bounds.cameraBoundsSize / 2, bounds.cameraBoundsSize), new Color(0, 0, 0, 0), Color.blue);
	}

	public override void OnInspectorGUI()
	{
		bounds = target as ActorBounds;


		EditorGUILayout.LabelField("Player Bounds", EditorStyles.boldLabel);

		EditorGUI.BeginChangeCheck();
		Vector2 _playerBoundsLocation = EditorGUILayout.Vector2Field("Position", bounds.playerBoundsPosition);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(bounds, "Player Move Bounds");
			bounds.playerBoundsPosition = _playerBoundsLocation;
			EditorUtility.SetDirty(bounds);
		}



		EditorGUI.BeginChangeCheck();
		Vector2 _playerBoundsSize = EditorGUILayout.Vector2Field("Size", bounds.playerBoundsSize);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(bounds, "Change Player Bounds Size");
			bounds.playerBoundsSize = _playerBoundsSize;
			EditorUtility.SetDirty(bounds);
		}

		EditorGUILayout.LabelField("Camera Bounds", EditorStyles.boldLabel);

		EditorGUI.BeginChangeCheck();
		Vector2 _CamerBoundsPosition = EditorGUILayout.Vector2Field("Position", bounds.cameraBoundsPosition);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(bounds, "Camera Move Bounds");
			bounds.cameraBoundsPosition = _CamerBoundsPosition;
			EditorUtility.SetDirty(bounds);
		}

		
		EditorGUI.BeginChangeCheck();
		Vector2 _cameraBoundsSize = EditorGUILayout.Vector2Field("Size", bounds.cameraBoundsSize);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(bounds, "Change Player Bounds Size");
			bounds.cameraBoundsSize = _cameraBoundsSize;
			EditorUtility.SetDirty(bounds);
		}
	}
	
}