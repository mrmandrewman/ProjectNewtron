﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
	private EnemyPath[] enemies;

	private int activeEnemies;

	private float waveDuration = 0;
	private float currentWaveTime = 0;
	private bool waveEnded = false;
	
	// Use this for initialization
	void Start()
	{
		// Get reference to all enemies within wave
		enemies = transform.GetComponentsInChildren<EnemyPath>();
		
		// Get number of active enemies
		activeEnemies = transform.GetComponentsInChildren<EnemyPath>(false).Length;
		
		// Calculate the wave duration
		foreach (EnemyPath Path in enemies)
		{
			if (Path.GetPathDuration() > waveDuration)
			{
				waveDuration = Path.GetPathDuration();
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!waveEnded)
		{
			currentWaveTime += Time.deltaTime;
			if (currentWaveTime >= waveDuration)
			{
				waveEnded = true;
				SendMessageUpwards("NextWave", SendMessageOptions.RequireReceiver);
			}
		}
		
	}

	public void EnemyDeactive()
	{
		activeEnemies -= 1;
		if (activeEnemies <= 0)
		{
			waveEnded = true;
			SendMessageUpwards("NextWave", SendMessageOptions.RequireReceiver);
		}
	}
	
	public bool GetWaveEnded()
	{
		return waveEnded;
	}
}
