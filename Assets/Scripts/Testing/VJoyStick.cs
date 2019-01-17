using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VJoyStick : MonoBehaviour {

    // Virtual Joystick Game Objects
    UnityEngine.UI.Image VJoyStickFrontImage;
    RectTransform VJoyStickFrontRect;
    UnityEngine.UI.Image VJoyStickBackImage;
   // RectTransform VJoyStickBackRect;

    public float VJoyStickPositionLimit = 0.3f;
    public float VJoyStickPositionMoveSpeed = 2.6f;

    // Virtual Joystick Input Variables
    bool inputPosition = false;
    //Vector2 inputInitPosition;
    Vector2 inputCurrentPosition;
    public Vector2 inputDeltaPosition;
    public float inputDeltaLimit = 1;
    public float inputDeadzone = 0.2f;


	// Use this for initialization
	void Start ()
    {
        VJoyStickFrontImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        VJoyStickFrontRect = transform.GetChild(0).GetComponent<RectTransform>();
        VJoyStickBackImage = GetComponent<UnityEngine.UI.Image>();
        //VJoyStickBackRect = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {

        UpdateVJoystick();
        UpdateVJoystickPosition();
        VJoystickVisibility();

    }

    void VJoystickVisibility()
    {
        if (inputPosition)
        {
            // Make the Joystick Visible
            VJoyStickFrontImage.enabled = true;
            VJoyStickBackImage.enabled = true;
        }
        else
        {
            // Make the Joystick Invisible
            VJoyStickFrontImage.enabled = false;
            VJoyStickBackImage.enabled = false;
        }
    }

    void UpdateVJoystick()
    {
        if (!inputPosition)
        {
            if (Input.touchCount != 0)
            {
                inputPosition = true;
                //inputInitPosition = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);

                // Initialise VJoystick Position
                
            }
        }
        else if (Input.touchCount != 0)
        {
            inputCurrentPosition = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
            // Calculate inputDelta
            inputDeltaPosition = inputCurrentPosition - new Vector2(transform.position.x, transform.position.y);
            inputDeltaPosition = inputDeltaPosition / inputDeltaLimit;

            if (inputDeltaPosition.magnitude <= inputDeadzone)
            {
                inputDeltaPosition = Vector2.zero;
            }

        }
        else
        {
            inputPosition = false;
            //inputInitPosition = Vector2.zero;
            inputCurrentPosition = Vector2.zero;
            inputDeltaPosition = Vector2.zero;
        }
    }

    void UpdateVJoystickPosition()
    {
        if (inputDeltaPosition.magnitude > 1 * VJoyStickPositionLimit)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(inputCurrentPosition.x, inputCurrentPosition.y), Time.deltaTime * VJoyStickPositionMoveSpeed);
            VJoyStickFrontRect.localPosition = inputDeltaPosition.normalized * VJoyStickPositionLimit;
        }
        else
        {
            VJoyStickFrontRect.localPosition = inputDeltaPosition;
        }
    }
}