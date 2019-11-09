using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActorGameMenu : MonoBehaviour
{
	public GameObject InGameUI;
	public GameObject PauseMenu;
	public GameObject GameOverMenu;
	public GameObject VictoryMenu;

	public static bool bGameIsPaused = false;


	private void OnEnable()
	{
		GameOverMenu.SetActive(false);
		PauseMenu.SetActive(false);
		VictoryMenu.SetActive(false);
	}

	public void PauseGame()
	{
		/* 
        Hides the in game UI and Shows the Pausemenu and sets time scale to 0 
        (If time is needed for other functionality
        Time.unscaledDeltaTime should be used)
        */
		PauseMenu.SetActive(true);
		PauseMenu.transform.GetComponentInChildren<Button>().Select();
		Time.timeScale = 0.0f;
		bGameIsPaused = true;

		InGameUI.transform.GetChild(2).GetComponent<Button>().interactable = false;
	}

	public void ResumeGame()
	{
		// Disables the pause menu and sets time back to normal
		PauseMenu.SetActive(false);
		Time.timeScale = 1.0f;
		ActorPlayer.CalibrateAccelerometer();
		ActorGameCamera.InitCameraSize();
		bGameIsPaused = false;


		InGameUI.transform.GetChild(2).GetComponent<Button>().interactable = true;
	}

	public void GameOver()
	{
		GameOverMenu.SetActive(true);
		Time.timeScale = 0.0f;
		GameOverMenu.transform.GetComponentInChildren<Button>().Select();

		InGameUI.transform.GetChild(2).GetComponent<Button>().interactable = false;
	}

	public void Retry()
	{
		Destroy(ActorLevelManager.instance);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void MainMenuButton()
	{
		// Return time to normal
		Time.timeScale = 1.0f;

		// Destroy the LevelManager so previous progress doesn't get saved
		Destroy(ActorLevelManager.instance);

		// Load the main menu
		SceneManager.LoadScene(0);
	}

	public void Victory()
	{
		VictoryMenu.SetActive(true);
		Time.timeScale = 0.0f;
		VictoryMenu.transform.GetComponentInChildren<Button>().Select();

		InGameUI.transform.GetChild(2).GetComponent<Button>().interactable = false;
	}
}
