using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class ActorPowerUp : MonoBehaviour
{
	[SerializeField]
	private Vector2 moveRate = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Rigidbody2D>().velocity = moveRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log(collision.gameObject);
			collision.gameObject.GetComponent<ShootingController>().PowerUp();
			Destroy(gameObject);
		}
	}

}
