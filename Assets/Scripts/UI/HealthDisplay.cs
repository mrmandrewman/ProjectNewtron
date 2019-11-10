using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

	private Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
		healthSlider = GetComponent<Slider>();
    }

	public void UpdateSlider()
	{
		healthSlider.value = ActorLevelManager.instance.GetPlayerCurrentHealth() / ActorLevelManager.instance.GetPlayerMaxHealth();
	}
}
