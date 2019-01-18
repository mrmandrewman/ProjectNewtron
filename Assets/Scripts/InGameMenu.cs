using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{

    public GameObject PauseMenu;
    public GameObject InGameUI;
	public Button PauseButton;

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
		PauseButton.interactable = false;
        Time.timeScale = 0.0f;
        bGameIsPaused = true;
    }

    public void ResumeGame()
    {
		// Disables the pause menu and sets time back to normal
		PauseMenu.SetActive(false);
		PauseButton.interactable = true;
		Time.timeScale = 1.0f;
		ActorPlayer.CalibrateAccelerometer();
		ActorCamera.InitCameraSize();
        bGameIsPaused = false;
    }
}
