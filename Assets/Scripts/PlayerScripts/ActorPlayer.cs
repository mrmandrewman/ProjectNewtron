using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType
{
	Touch,
	Tilt
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ActorPlayer : ShipController
{


	public float moveSpeedPrototype = 3.0f;

	// Unused kept as comment form for now
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
	[SerializeField] Bounds boundary;

	// Move Speed for Tilt Controls
	[SerializeField] float moveSpeedTilt = 5.0f;

	// Power to increase for precision controls
	[SerializeField] float precisionControlPower = 1.6f;

	// Used to Calibrate Device Tilt
	public static Matrix4x4 calibrationMatrix;

	// Deadzone Magnitudes
	[SerializeField] float wantedDeadZone = 0.05f;

	// Magnitude of tilt limit
	[SerializeField] float wantedTiltLimit = 0.25f;

	// Magnitude for Clamp Max
	[SerializeField] float wantedMaxClamp = 0.05f;
	#endregion


	//public int attackPowerUpsCollected = 0;
	//public int attackSpeedPowerUpsCollected;

	// Game objects where bullets spawn from
	[SerializeField] GameObject[] TripleTurrets;
	[SerializeField] GameObject[] QuinTurrets;

	// Use this for initialization
	void Start()
	{
		CalibrateAccelerometer();

		// Initialise from PlayerState
		currentHealth = PlayerState.SharedInstance.maxHealth;
		currentShield = PlayerState.SharedInstance.maxShield;

		//attackPowerUpsCollected = PlayerState.SharedInstance.attackPowerUpsCollected;

		// Start autoshooting
		StartCoroutine("Shoot");
	}

	// Update is called once per frame
	void Update()
	{
		// Accelerometer Input
		InputDir = GetAccelerometer(Input.acceleration);
		InputDir.z = 0;

		// Check for floating point error
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
		PrototypeControls();

		// Touch Controls
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


	// For Development only
	void PrototypeControls()
	{
		transform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeedPrototype * Time.deltaTime);
	}

	// Coroutine to shoot bullets based on current upgrade
	override public IEnumerator Shoot()
	{
		while (bShotActive)
		{
			switch (CurrentAttackType)
			{
				case AttackType.Basic:
					TrySpawnBullet(BasicTurret);
					break;
				case AttackType.Triple:
					for (int i = 0; i < TripleTurrets.Length; i++)
					{
						TrySpawnBullet(TripleTurrets[i]);
					}
					break;
				case AttackType.Quin:
					for (int i = 0; i < QuinTurrets.Length; i++)
					{
						TrySpawnBullet(QuinTurrets[i]);
					}
					break;
				default:
					break;
			}
			yield return new WaitForSeconds(shotReloadTime);
		}
	}

	// Spawns a bullet at the referenced gameobjects transform, tries to dip into object pool
	override public bool TrySpawnBullet(GameObject gameObject)
	{
		GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject();
		if (bullet != null)
		{
			bullet.transform.position = gameObject.transform.position;
			bullet.transform.rotation = gameObject.transform.rotation;
			bullet.SetActive(true);
			bullet.GetComponent<Projectile>().ignoreTag = "Player";
			return true;
		}
		else
		{
			ObjectPooler.SharedInstance.AddToPool();
			TrySpawnBullet(gameObject);
		}
		return false;
	}

}
