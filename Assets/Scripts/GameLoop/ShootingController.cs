using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

	// publicly accessable boolean to turn shooting off and on
	[SerializeField]
	private bool bShotActive = false;

	// How quickly the actor can shoot without powerups
	public float shotReloadTime = 0.1f;

	// How quickly the actor can shoot with powerups
	private float currentReloadTime = 0.1f;

	[SerializeField]
	private GameObject projectile = null;

	[SerializeField]
	private Transform turret = null;

	private bool isShooting = false;

	// Called using awake so sets the currentReload Time before the actorLevelManager makes any changes necesarry 
	private void Awake()
	{
		currentReloadTime = shotReloadTime;
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

	public void SetCurrentReloadSpeed(float newReloadSpeed)
	{
		currentReloadTime = newReloadSpeed;
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
