using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActorGameMenu : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject InGameUI;

    public static bool bGameIsPaused = false;

	private void Start()
	{
		PauseGame();
	}
	// Update is called once per frame
	void Update()
    {

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

    }

    public void ResumeGame()
    {
		// Disables the pause menu and sets time back to normal
		PauseMenu.SetActive(false);
		Time.timeScale = 1.0f;
		ActorPlayer.CalibrateAccelerometer();
		ActorGameCamera.InitCameraSize();
        bGameIsPaused = false;
    }

	public void MainMenuButton()
	{
		// Return time to normal
		Time.timeScale = 1.0f;

		// Destroy the LevelManager so previous progress doesn't get saved
		Destroy(ActorLevelManager.instance.gameObject);

		// Load the main menu
		SceneManager.LoadScene(0);
	}
}
