using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour
{
	private string scoreString;

	private Text scoreText;

	private void Start()
	{
		scoreText = GetComponent<Text>();
	}

	public void DisplayScore()
	{
		scoreString = "SCORE: " + ActorLevelManager.instance.GetScore(8);
		scoreText.text = scoreString;
	}
}
