using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : DamageController
{
	public override void TakeDamage(float _damage)
	{
		ActorLevelManager.instance.ChangePlayerCurrentHealth(-_damage);
		if (currentHealth <= 0)
		{
			SendMessage("Death");
		}
	}
}
