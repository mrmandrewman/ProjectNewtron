using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(ShootingController))]
[RequireComponent(typeof(DamageController))]
public class ActorEnemy : MonoBehaviour
{
	[SerializeField]
	private ActorEnemyPath shipPath;
	private float progress = 0;
	private bool isFinished = false;

	[SerializeField, Range(0, 1)]
	private float powerUpDropChance = 0.05f;
	[SerializeField]
	private GameObject powerUpDrop;

	[SerializeField]
	private int scoreValue = 100;

	private ShootingController myShootingControler;

	private void Start()
	{
		myShootingControler = GetComponent<ShootingController> ();
	}

	// Update is called once per frame
	void Update()
	{
		if (!isFinished)
		{
			progress += Time.deltaTime;
			transform.localPosition = shipPath.transform.InverseTransformPoint(shipPath.GetPathPoint(progress, out isFinished));
		}
		else
		{
			RemoveFromWave();
		}

		if (!myShootingControler.enabled && progress > shipPath.GetDelayTime(0))
		{
			myShootingControler.enabled = true;
		}
	}

	private void Death()
	{
		// Add Points to player and set as inactive
		ActorLevelManager.instance.AddPoints(scoreValue);

		// Chance to drop power up
		TrySpawnPowerUp();

		// Spawn death particle effect

		// Change sprite colour to start explosion
		RemoveFromWave();
	}

	private void TrySpawnPowerUp()
	{
		float diceRoll = Random.Range(0.0f, 1.0f);
		if (diceRoll <= powerUpDropChance)
		{
			Instantiate(powerUpDrop,transform.position,transform.rotation);
		}
	}

	private void RemoveFromWave()
	{

		SendMessageUpwards("EnemyDeactive", SendMessageOptions.DontRequireReceiver);
		transform.parent.gameObject.SetActive(false);
	}
	
}
