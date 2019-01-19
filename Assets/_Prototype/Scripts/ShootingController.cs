using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour {

	// publicly accessable boolean to turn shooting off and on
	public bool bShotActive = true;

	// How quickly the actor can shoot
	public float shotReloadTime = 0.1f;

	private void Awake()
	{
		// Start autoshooting
		StartCoroutine("Shooting");
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	virtual public IEnumerator Shooting()
	{
		while (bShotActive)
		{
			gameObject.SendMessage("Shoot", SendMessageOptions.RequireReceiver);

			yield return new WaitForSeconds(shotReloadTime);
		}
	}
}
