using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorProjectile : MonoBehaviour
{
    public string ignoreTag = "";
    public float damage = 1.0f;
    public float speed = 5;
    public float TTL = 2.0f;
    private float CurrentTTL;


    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        CurrentTTL = TTL;
    }


    private void OnDisable()
    {
        
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CurrentTTL -= Time.deltaTime;
        if (CurrentTTL <= 0.0f)
        {            
            Destroy(gameObject);
        }
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != ignoreTag)
        {
            collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
        }

		if (collision.gameObject.name == "DeactivateProjectiles")
		{
			Destroy(gameObject);
		}
    }


}
