using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorWaveManager : MonoBehaviour
{
	private ActorEnemyWave[] waves;
	
	private int currentWave = 0;
	
	// To be used for level manager
	// private bool levelFinished = false;


	// Use this for initialization
	void Start()
	{
		// Get reference to all waves that are children of this object
		waves = transform.GetComponentsInChildren<ActorEnemyWave>(true);

		waves[currentWave].gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void NextWave()
	{
		// Disable wave just finished
		waves[currentWave].gameObject.SetActive(false);

		// Set next wave to current wave and enable
		currentWave++;
		if (currentWave < waves.Length)
		{
			waves[currentWave].gameObject.SetActive(true);
		}
		else
		{
			ActorLevelManager.instance.Victory();
		}
	}
}
