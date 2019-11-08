using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGameCamera : MonoBehaviour
{
	// The player object that the camera will follow
	private GameObject player;

	// The space where the player can move around and the camera will not move
	SphereCollider deadZone;
	// The desired move translation of the camera
	Vector3 moveVector;

	// The boundary of the level
	public ActorBounds boundary;

	public static Vector2 cameraSize;
	private Vector2 cameraLowerClamp;
	private Vector2 cameraUpperClamp;

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		deadZone = GetComponent<SphereCollider>();

		InitCameraSize();

		// Calculate the lower bounds for the camera clamp
		cameraLowerClamp.x = boundary.cameraBoundsPosition.x - boundary.cameraBoundsHalfSize.x + cameraSize.x;
		cameraLowerClamp.y = boundary.cameraBoundsPosition.y - boundary.cameraBoundsHalfSize.y + cameraSize.y;


		// Calculate the upper bounds for the camera clamp
		cameraUpperClamp.x = boundary.cameraBoundsPosition.x + boundary.cameraBoundsHalfSize.x - cameraSize.x;
		cameraUpperClamp.y = boundary.cameraBoundsPosition.y + boundary.cameraBoundsHalfSize.y - cameraSize.y;
	}


	private void FixedUpdate()
	{

		// Checks to see if player has left the dead zone
		if (!deadZone.bounds.Contains(player.transform.position))
		{
			// Calculate how the camera moves based on the closest point on the collider from the player
			moveVector = player.transform.position - deadZone.bounds.ClosestPoint(player.transform.position);

			transform.position = Vector3.Lerp(transform.position, transform.position + moveVector, 0.05f);
		}

		// Clamps the camera position
		transform.position = new Vector3
		(
			Mathf.Clamp(transform.position.x, cameraLowerClamp.x, cameraUpperClamp.x),
			Mathf.Clamp(transform.position.y, cameraLowerClamp.y, cameraUpperClamp.y),
			transform.position.z
		);
	}

	// Calculate the orthographic half height and width of the camera
	public static void InitCameraSize()
	{
		cameraSize.y = Camera.main.orthographicSize;
		cameraSize.x = cameraSize.y * Camera.main.aspect;
	}
}
