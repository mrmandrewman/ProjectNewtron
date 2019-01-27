using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBounds : MonoBehaviour
{
	// player bounds variables
	[SerializeField]
	public Vector2 playerBoundsSize;
	[SerializeField]
	public Vector2 playerBoundsPosition;

	// camera bounds position
	[SerializeField]
	public Vector2 cameraBoundsSize;
	[SerializeField]
	public Vector2 cameraBoundsPosition;

	public Vector2 playerBoundsHalfSize;
	public Vector2 cameraBoundsHalfSize;

	public void Start()
	{
		playerBoundsHalfSize = playerBoundsSize / 2;
		cameraBoundsHalfSize = cameraBoundsSize / 2;
	}
}
