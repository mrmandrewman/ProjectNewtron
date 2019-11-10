using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorLevelManager : MonoBehaviour
{
	public static ActorLevelManager instance = null;

	// 
	public ActorGameMenu gameMenu = null;
	public UnityEngine.Events.UnityEvent scoreDisplay;
	public ShootingController playerShootingController = null;
	public DamageController playerDamageController = null;
	public HealthDisplay playerHealthDisplay = null;
	// Game Variables
	static int currentScore = 0;

	// How many power ups the player has picked up
	static float playerReloadSpeed = 0;

	static float playerCurrentHealth;
	static float playerMaxHealth;

	void Start()
	{
		//Check if instance already exists
		if (instance == null)
		{
			//if not, set instance to this
			instance = this;

			playerReloadSpeed = playerShootingController.shotReloadTime;
			playerMaxHealth = playerDamageController.maxHealth;
			playerCurrentHealth = playerDamageController.currentHealth;
		
		}
		//If instance already exists and it's not this
		else if (instance != this)
		{
			// Set the references of the instance that already exists to references associated with this Level Manager
			instance.gameMenu = gameMenu;
			instance.scoreDisplay = scoreDisplay;
			instance.playerShootingController = playerShootingController;
			instance.playerHealthDisplay = playerHealthDisplay;
			// Set the reload speed of the player to what's stored in the instance without affecting the firerate
			instance.changeReloadSpeed(0);

			//Destroy this, only allowing one instance
			Destroy(gameObject);
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}


	// Method to alter points called by other scripts
	public void AddPoints(int _points)
	{
		// add points to current score (points can be negative values)
		currentScore += _points;

		// prevent current score from dropping below zero
		if (currentScore <= 0)
		{
			currentScore = 0;
		}
		scoreDisplay.Invoke();
	}

	public int GetScore()
	{
		return currentScore;
	}

	// Returns the current score with specified number of digits, if digits entered is less than num of digits of score, returns score without any proceeding digits.
	public string GetScore(int _digits)
	{
		string scoreString = currentScore.ToString();
		if (_digits <= scoreString.Length)
		{
			return scoreString;
		}

		return scoreString.PadLeft(_digits, '0');
	}

	public void changeReloadSpeed(float _reloadMultiplier)
	{
		playerReloadSpeed *= _reloadMultiplier;
		playerShootingController.SetCurrentReloadSpeed(playerReloadSpeed);
	}

	public float GetFireRate()
	{
		return playerReloadSpeed;
	}

	public void ChangePlayerCurrentHealth(float deltaHealth)
	{
		playerCurrentHealth += deltaHealth;
		playerDamageController.currentHealth = playerCurrentHealth;
		playerHealthDisplay.UpdateSlider();
	}

	public float GetPlayerCurrentHealth()
	{
		return playerCurrentHealth;
	}

	public float GetPlayerMaxHealth()
	{
		return playerMaxHealth;
	}

	public void GameOver()
	{
		gameMenu.GameOver();
	}

	public void Victory()
	{
		gameMenu.Victory();
	}
}
