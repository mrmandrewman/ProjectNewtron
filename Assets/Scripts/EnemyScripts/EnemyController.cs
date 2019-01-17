using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ShipController
{
	public EnemyPath shipPath;
	private float progress = 0;
	private bool isFinished = false;

	// Use this for initialization
	void Start()
	{
		// Start autoshooting
		StartCoroutine("Shoot");
	}

	void OnEnabled()
	{
	}

	private void OnEnable()
	{
		currentHealth = maxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isFinished)
		{
			progress += Time.deltaTime;
			transform.localPosition = shipPath.transform.InverseTransformPoint(shipPath.GetPathPoint(progress, out isFinished));
		}

		if (isFinished)
		{
			Death();
		}

		if (currentHealth <= 0)
		{
			// ToDo Add points to player
			Death();
		}
	}

	public override bool TrySpawnBullet(GameObject gameObject)
	{
		GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();
		if (bullet != null)
		{
			bullet.transform.position = gameObject.transform.position;
			bullet.transform.rotation = gameObject.transform.rotation;
			bullet.SetActive(true);
			bullet.GetComponent<Projectile>().ignoreTag = "Enemy";
			return true;
		}
		else
		{
			ObjectPooler.SharedInstance.AddToPool();
			TrySpawnBullet(gameObject);
		}
		return false;
	}

	protected override void Death()
	{
		// Add Points to player and set as inactive
		SendMessageUpwards("EnemyDeactive", SendMessageOptions.DontRequireReceiver);
		transform.parent.gameObject.SetActive(false);
	}



}
