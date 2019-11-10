using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class DamageController : MonoBehaviour {

	public float maxHealth;
	[HideInInspector]
	public float currentHealth;
	
	private void Awake()
	{
		currentHealth = maxHealth;

	}

	public virtual void TakeDamage(float _damage)
	{
		currentHealth -= _damage;
		if (currentHealth <= 0)
		{
			SendMessage("Death");
		}
		
	}
}
