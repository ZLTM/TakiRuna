using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class RotationController : MonoBehaviour {

	public GameObject GM;
	public GameObject myo = null;

	private Pose lastPose;
	private ThalmicMyo thalmicMyo;
	private ThalmicHub hub;

	private Quaternion antiYaw = Quaternion.identity;
	private float referenceRoll = 0.0f;


	void Start () {
		lastPose = Pose.Unknown;
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		hub = ThalmicHub.instance;
	}

	void Update () {
		bool resetYRotation = false;
		if (GM.GetComponent<GMController>().isDying) {
			return;
		}
		if (thalmicMyo.pose != lastPose) {
			lastPose = thalmicMyo.pose;
			if (thalmicMyo.pose == Pose.WaveIn) {
				resetYRotation = true;
				ExtendUnlockAndNotifyUserAction(thalmicMyo);
			}
		}

		if (Input.GetKeyDown ("r")) {
			resetYRotation = true;
		}

		if (resetYRotation) {
			Vector3 parentOrientation = transform.parent.forward.normalized;
			antiYaw = Quaternion.FromToRotation (
				new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
				new Vector3 (parentOrientation.x, 0, parentOrientation.z)
			);
			Vector3 referenceZeroRoll = computeZeroRollVector (myo.transform.forward);
			referenceRoll = rollFromZero (referenceZeroRoll, myo.transform.forward, myo.transform.up);
		}

		// Current zero roll vector and roll value.
		Vector3 zeroRoll = computeZeroRollVector (myo.transform.forward);
		float roll = rollFromZero (zeroRoll, myo.transform.forward, myo.transform.up);

		float relativeRoll = normalizeAngle (roll - referenceRoll);

		// antiRoll represents a rotation about the myo Armband's forward axis adjusting for reference roll.
		Quaternion antiRoll = Quaternion.AngleAxis (relativeRoll, myo.transform.forward);

		// Here the anti-roll and yaw rotations are applied to the myo Armband's forward direction to yield
		// the orientation of the joint.
		transform.rotation = antiYaw * antiRoll * Quaternion.LookRotation (myo.transform.forward);

		// Compensate if Myo
		if (thalmicMyo.xDirection == Thalmic.Myo.XDirection.TowardWrist) {
			transform.rotation = new Quaternion(transform.localRotation.x,
				-transform.localRotation.y,
				transform.localRotation.z,
				-transform.localRotation.w);
		}

	}

	/**
	 * Returns a unitary perpendicular vector to the Myo fwd direction, without considering myo rotation
	 */
	Vector3 computeZeroRollVector (Vector3 forward) {
		Vector3 antigravity = Vector3.up;
		Vector3 m = Vector3.Cross (myo.transform.forward, antigravity);
		Vector3 roll = Vector3.Cross (m, myo.transform.forward);

		return roll.normalized;
	}
		
	/**
	 * Returns a unitary perpendicular vector to the Myo fwd direction, without considering myo rotation
	 */
	float rollFromZero (Vector3 zeroRoll, Vector3 forward, Vector3 up) {
		float cosine = Vector3.Dot (up, zeroRoll);

		Vector3 cp = Vector3.Cross (up, zeroRoll);
		float directionCosine = Vector3.Dot (forward, cp);
		float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

		return sign * Mathf.Rad2Deg * Mathf.Acos (cosine);
	}

	/**
	 * Returns angle normalized between 180 and -180
	 */
	float normalizeAngle (float angle) {
		if (angle > 180.0f) {
			return angle - 360.0f;
		}
		if (angle < -180.0f) {
			return angle + 360.0f;
		}
		return angle;
	}

	/**
	 * Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given Myo that a user action was recognized
	 */
	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo) {
		ThalmicHub hub = ThalmicHub.instance;

		if (hub.lockingPolicy == LockingPolicy.Standard) {
			myo.Unlock (UnlockType.Timed);
		}

		myo.NotifyUserAction ();
	}
}
