using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType
{
	Touch,
	Tilt
}

public enum AttackType
{
	None,
	Basic,
	Triple,
	Quin,
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(ShootingController))]
[RequireComponent(typeof(DamageController))]
public class ActorPlayer : MonoBehaviour
{

	//private AttackType CurrentAttackType = AttackType.Basic;

	public GameObject bullet;
	public GameObject basicTurret;

	public float moveSpeedPrototype = 3.0f;

	// Unused kept as comment for now
	#region Touch Controls Parameters
	// Reference to players touch for input
	//Touch touch;

	// Move speed parameters
	//public float moveSpeedTouch = 0.1f;
	#endregion


	#region Tilt Controls Parameters
	//Accelerometer input
	Vector3 InputDir;

	// The boundary of the map that the player can move to
	public ActorBounds boundary;

	// Move Speed for Tilt Controls
	[SerializeField] float moveSpeedTilt = 5.0f;

	// Power to increase for precision controls
	[SerializeField] float precisionControlPower = 1.6f;

	// Used to Calibrate Device Tilt
	private static Matrix4x4 calibrationMatrix;

	// Deadzone Magnitudes
	[SerializeField] float wantedDeadZone = 0.05f;

	// Magnitude of tilt limit
	[SerializeField] float wantedTiltLimit = 0.25f;

	// Magnitude for Clamp Max
	[SerializeField] float wantedMaxClamp = 0.05f;
	#endregion

	
	// Use this for initialization
	void Start()
	{
		CalibrateAccelerometer();
		gameObject.GetComponent<ShootingController>().StartCoroutine("Shooting");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		// Accelerometer Input
		InputDir = GetAccelerometer(Input.acceleration);
		InputDir.z = 0;

		// Check for floating point error and apply input
		if (Mathf.Abs((InputDir * moveSpeedTilt * Time.deltaTime).magnitude) > 0.02)
		{
			transform.Translate(InputDir * moveSpeedTilt * Time.deltaTime);
		}


		// Clamps position to boundaries
		transform.position = new Vector3
		(
			Mathf.Clamp(transform.position.x, boundary.playerBoundsPosition.x - boundary.playerBoundsHalfSize.x, boundary.playerBoundsPosition.x + boundary.playerBoundsHalfSize.x),
			Mathf.Clamp(transform.position.y, boundary.playerBoundsPosition.y - boundary.playerBoundsHalfSize.y, boundary.playerBoundsPosition.y + boundary.playerBoundsHalfSize.y),
			0.0f
		);


		// For Development only
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			transform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeedPrototype * Time.deltaTime);
		}

		/* Touch Controls
		//if (Input.touchCount > 0)
		//{
		//    touch = Input.GetTouch(0);
		//    switch (touch.phase)
		//    {
		//        case TouchPhase.Moved:
		//            GetComponent<Rigidbody2D>().AddForce(touch.deltaPosition * moveSpeedTouch);
		//            break;
		//        default:
		//            break;
		//    }
		//}
		*/

	}

	#region Tilt Controls Methods

	//Method for calibration 
	public static void CalibrateAccelerometer()
	{
		Vector3 initialRotation = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), initialRotation);
		//create identity matrix ... rotate our matrix to match up with down vec
		Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
		//get the inverse of the matrix
		calibrationMatrix = matrix.inverse;

	}

	//Method to get the calibrated input 
	Vector3 GetAccelerometer(Vector3 accelerator)
	{
		// Gets current tilt in relation to calibrated tilt
		Vector3 accel = calibrationMatrix.MultiplyVector(accelerator);

		// Convert Cartesian Coordinates to Polar Coordinates
		Vector2 polarAccel = new Vector2(Mathf.Sqrt((accel.x * accel.x) + (accel.y * accel.y)), Mathf.Atan2(accel.y, accel.x));

		// Apply Deadzone
		polarAccel.x = polarAccel.x - wantedDeadZone;
		if (polarAccel.x <= 0)
		{
			// Removes any negative value so that dead zone applies properly
			polarAccel.x = 0;
		}

		// Apply Tilt Limit
		polarAccel.x = polarAccel.x / wantedTiltLimit;

		// Apply Max Clamp and Precion Control by changing the input from a linear relationship to quadratic relationship
		polarAccel.x = Mathf.Pow((polarAccel.x + wantedMaxClamp), precisionControlPower);


		// Limit output to maximum of 1 so that maximum clamp applies
		if (polarAccel.x >= 1)
		{
			polarAccel.x = 1;
		}

		// Convert Polar coordinates back to Cartesian coordinates
		accel.x = polarAccel.x * Mathf.Cos(polarAccel.y);
		accel.y = polarAccel.x * Mathf.Sin(polarAccel.y);


		return accel;
	}

	#endregion

	public void Death()
	{
		gameObject.SetActive(false);
	}
}
