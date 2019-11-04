using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorLevelManager : MonoBehaviour
{
	public static ActorLevelManager instance = null;


	// Game Variables
	int currentScore = 0;

	// UI Elements
	public UnityEngine.UI.Text scoreDisplay;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)
		{
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);
		}

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{

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
		scoreDisplay.text = GetScore(8);

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
}
