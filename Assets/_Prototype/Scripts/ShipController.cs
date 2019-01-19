using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShipController : MonoBehaviour
{
	[Header("Stats")]
    public float maxHealth;
    protected float currentHealth;
    public float maxShield;
    protected float currentShield;
    public float shieldRechargeRate = 0.1f;
    public float shieldRechargeDelay = 0.1f;
    protected bool takenDamage = false;
    // Attack parameters
	

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;
    }

}
