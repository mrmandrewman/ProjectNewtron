﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBGScroller : MonoBehaviour
{
	public float scrollSpeed;
	public float tileSizeZ;

	private Vector3 startPosition;

	// Use this for initialization
	void Start()
	{
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
		//Debug.Log(newPosition);
		 transform.position = startPosition + Vector3.down * newPosition;
	}
}
