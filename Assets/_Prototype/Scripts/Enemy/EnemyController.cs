using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(ShootingController))]
[RequireComponent(typeof(DamageController))]
public class EnemyController : MonoBehaviour
{
	public EnemyPath shipPath;
	private float progress = 0;
	private bool isFinished = false;
	[SerializeField] GameObject turret;
	[SerializeField] GameObject bullet;


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

	private bool Shoot()
	{
		//GameObject bulletClone = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
		//bulletClone.GetComponent<Projectile>().ignoreTag = "Enemy";
		return false;
	}

	private void Death()
	{
		// Add Points to player and set as inactive
		SendMessageUpwards("EnemyDeactive", SendMessageOptions.DontRequireReceiver);
		transform.parent.gameObject.SetActive(false);
	}

}
