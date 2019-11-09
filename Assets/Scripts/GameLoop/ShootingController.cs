using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

	// publicly accessable boolean to turn shooting off and on
	[SerializeField]
	private bool bShotActive = false;

	// How quickly the actor can without powerups
	[SerializeField]
	private float shotReloadTime = 0.1f;
	[SerializeField, Tooltip("How much powerups multiply the reload speed")]
	private float powerUpPower = 0.95f;

	private float currentReloadTime;

	[SerializeField]
	private GameObject projectile;

	[SerializeField]
	private Transform turret;

	private bool isShooting = false;

	private void Start()
	{
		currentReloadTime = shotReloadTime * Mathf.Pow(powerUpPower, ActorLevelManager.instance.GetPowerLevel());
	}

	private void OnEnable()
	{

	}

	public IEnumerator Shooting()
	{
		isShooting = true;
		while (bShotActive)
		{
			GameObject projectileClone = Instantiate(projectile, turret.position, turret.rotation);
			projectileClone.GetComponent<ActorProjectile>().ignoreTags.Add(gameObject.tag);
			yield return new WaitForSeconds(currentReloadTime);
		}
		isShooting = false;
	}

	public void PowerUp()
	{
		ActorLevelManager.instance.AddPowerUp(1);
		currentReloadTime = shotReloadTime * Mathf.Pow(powerUpPower, ActorLevelManager.instance.GetPowerLevel());

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "EnemyShootingZone")
		{
			if (isShooting)
			{
				StopAllCoroutines();
			}
			else
			{
				StartCoroutine("Shooting");
			}
		}
	}
}
