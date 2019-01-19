using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(ShootingController))]
public class EnemyController : MonoBehaviour
{
	public EnemyPath shipPath;
	private float progress = 0;
	private bool isFinished = false;

	[SerializeField] float maxHealth;
	protected float currentHealth;

	// Use this for initialization
	void Start()
	{

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

	private bool shoot(GameObject gameObject)
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
			shoot(gameObject);
		}
		return false;
	}

	private void Death()
	{
		// Add Points to player and set as inactive
		SendMessageUpwards("EnemyDeactive", SendMessageOptions.DontRequireReceiver);
		transform.parent.gameObject.SetActive(false);
	}



}
