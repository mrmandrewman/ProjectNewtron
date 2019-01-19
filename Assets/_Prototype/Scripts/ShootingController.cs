using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {
	[Tooltip("Actor with a shoot function to spawn projectile")]
	[SerializeField] MonoBehaviour shootingActor;

	// publicly accessable boolean to turn shooting off and on
	public bool bShotActive = true;

	// How quickly the actor can shoot
	public float shotReloadTime = 0.1f;

	private void Awake()
	{
		// Start autoshooting
		StartCoroutine("Shoot");
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	virtual public IEnumerator Shoot()
	{
		while (bShotActive)
		{
			shootingActor.SendMessage("shoot", SendMessageOptions.RequireReceiver);

			yield return new WaitForSeconds(shotReloadTime);
		}
	}
}
