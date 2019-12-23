using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDamageController : DamageController
{
	[Header("_Required Affected Components")]
	[SerializeField]
	private SpriteRenderer playerSpriteRenderer = null;


	[Header("Player Hit Controls")]
	[SerializeField, Tooltip("How long the player is invinsible for after taking damage, in seconds")]
	private float hitInvinsibilityTime = 0.5f;
	public bool b_Invinsible = false;
	private float currentHitTime = 0.0f;
	[SerializeField, Range(0.01f, 1.0f)]
	private float hitFlashTime = 0.05f;
	private float currentFlashTime = 0;

	public override void TakeDamage(float _damage)
	{
		// Exits function if player is invinsible
		if (b_Invinsible)
		{
			return;
		}

		StartInvincibility();

		//ActorLevelManager.instance.ChangePlayerCurrentHealth(-_damage);
		if (currentHealth <= 0)
		{
			SendMessage("Death");
		}

	}

	private void StartInvincibility()
	{
		b_Invinsible = true;
		StartCoroutine("PlayerHit");
	}

	IEnumerator PlayerHit()
	{
		// Only flashes when invincible
		while (b_Invinsible)
		{
			print("Changing");
			// Keeps track of the time since the player has been hit
			currentHitTime += Time.deltaTime;

			// Used to drive and track the flash
			currentFlashTime += Time.deltaTime;
			if (currentFlashTime >= hitFlashTime)
			{
				currentFlashTime = 0;
			}
			float currentFlashAlpha = currentFlashTime / hitFlashTime;
			// Change the alpha of the sprite renderer
			Color targetColour = playerSpriteRenderer.color;
			targetColour.a = Mathf.Abs(currentFlashAlpha);
			playerSpriteRenderer.color = targetColour;



			// if the time since the player has been hit has reached the time the player can be invincible
			if (currentHitTime >= hitInvinsibilityTime)
			{
				// reset the hit time for the next time the player gets hit
				currentHitTime = 0;
				// reset flash time
				currentFlashTime = 0;
				// make sprite renderer completely opaque
				targetColour.a = 1;
				playerSpriteRenderer.color = targetColour;

				// the player is no longer invincible
				b_Invinsible = false;

			}

			// Waits until the next 
			yield return new WaitForEndOfFrame();
		}
	}
}
