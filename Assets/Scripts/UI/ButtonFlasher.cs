using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFlasher : MonoBehaviour
{
	[SerializeField]
	private Text flashingText = null;
	[SerializeField]
	private Color[] flashColours = null;
	private int currentColourIndex = 0;
	[SerializeField, Tooltip("The time inbetween changing colour")]
	private float flashIntervalTime = 0.0f;
	private float currentIntervalTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		currentIntervalTime += Time.unscaledDeltaTime;
		if (currentIntervalTime >= flashIntervalTime)
		{
			Debug.Log("Swap colour to colour " + currentColourIndex);
			// Reset interval time
			currentIntervalTime = 0;

			// Increase the colour index to set the next colour in array
			currentColourIndex++;
			// Reset index if it goes out of array bounds
			if (currentColourIndex >= flashColours.Length)
			{
				currentColourIndex = 0;
			}
			// Change button colour to the next colour
			flashingText.color = flashColours[currentColourIndex];
		}
        
    }
	
}
