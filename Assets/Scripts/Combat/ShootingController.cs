using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

	// publicly accessable boolean to turn shooting off and on
	public bool bShotActive = false;

	// How quickly the actor can shoot
	public float shotReloadTime = 0.1f;

	[SerializeField]
	private GameObject projectile;

	[SerializeField]
	private Transform turret;

	private void OnEnable()
	{
		StartCoroutine("Shooting");
	}

	virtual public IEnumerator Shooting()
	{
		while (bShotActive)
		{
			GameObject projectileClone = Instantiate(projectile, turret.position, turret.rotation);
			projectileClone.GetComponent<ActorProjectile>().ignoreTag = gameObject.tag;
			yield return new WaitForSeconds(shotReloadTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "EnemyShootingZone")
		{
			if (bShotActive)
			{
				bShotActive = false;
				StopAllCoroutines();
				Debug.Log("Stop Shooting");
			}
			else
			{
				bShotActive = true;
				StartCoroutine("Shooting");
				Debug.Log("Start Shooting");
			}
		}
	}
}
