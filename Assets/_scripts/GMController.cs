using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using VibrationType = Thalmic.Myo.VibrationType;

public class GMController : MonoBehaviour {

	public GameObject myo = null;
	public GameObject playerBody;
	public GameObject uiAlert;
	public GameObject uiGameOver;
	public bool isDying;
	public AudioClip cCosa;
	public AudioClip cDead;
	public AudioClip gameOverSong;
	public GameObject playerPart;

	private ThalmicHub hub;
	private ThalmicMyo thalmicMyo;
	private bool isPaused;
	private bool onTime;
	private bool didRitual;

	private Pose lastPose;

	void Start () {
		hub = ThalmicHub.instance;
		thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		isPaused = false;
		onTime = true;
		didRitual = false;
		isDying = false;
		Time.timeScale = 1f;
		uiGameOver.SetActive (false);

		lastPose = Pose.Unknown;
	}

	void Update () {
		string alert = "";

		//Die
		if (isDying) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Application.LoadLevel ("01-menu");
				return;
			}
			if (Input.GetKeyDown(KeyCode.R)) {
				Application.LoadLevel ("02-lvl01");
				return;
			}
			if (thalmicMyo.pose != lastPose) {
				lastPose = thalmicMyo.pose;
				if (thalmicMyo.pose == Pose.WaveOut) {
					Application.LoadLevel ("02-lvl01");
					return;
				}
			}
			return;
		}

		//Reconnect to Myo
		if (Input.GetKeyDown ("q")) {
			hub.ResetHub();
		}

		//Verifying Connections
		if (!hub.hubInitialized) {
			alert = "Cannot connect to Myo\n" +
				"Press Q to try again";
		} else if (!thalmicMyo.isPaired) {
			alert = "No Myo currently paired.";
		} else if (!thalmicMyo.armSynced) {
			alert = "Perform the Sync Gesture.";
		} else {
			alert = "";
		}
		//Pause, unpause game if necesary
		if (!string.IsNullOrEmpty(alert)) {
			//pauseAndShowAlert(alert);
			isPaused =  true;
		} else if (isPaused) {
			//unpause();
		}
	}

	void pauseAndShowAlert (string alert) {
		Time.timeScale = 0.0f;
		uiAlert.SetActive(true);
		uiAlert.GetComponent<Text>().text = alert;
	}

	void unpause() {
		Time.timeScale = 1.0f;
		uiAlert.SetActive(false);
		uiAlert.GetComponent<Text>().text = "";
	}

	public void setOnTime(bool flag) {
		onTime = flag;
	}
	public bool getOnTime() {
		return onTime;
	}

	public void setDidRitual(bool flag) {
		didRitual = flag;
	}
	public bool getDidRitual() {
		return didRitual;
	}

	public void gameOver() {
		if (!isDying) {
			isDying = true;
			Debug.Log ("Dead");
			//thalmicMyo.Vibrate (VibrationType.Long);
			playerBody.GetComponent<Rigidbody> ().AddTorque(new Vector3(-15,0,0));
			playerBody.GetComponent<Rigidbody> ().AddForce(Vector3.down*10);
			//playerBody.transform.FindChild ("BodyArm").GetComponent<ArmController> ().enabled = false;
			StartCoroutine(dead());
		}
	}

	IEnumerator dead() {
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource> ().PlayOneShot (cCosa);
		yield return new WaitForSeconds(1.5f);
		Time.timeScale = 0.5f;
		playerPart.transform.Find ("Blood").gameObject.SetActive(true);
		GetComponent<AudioSource> ().PlayOneShot (cDead);
		GetComponent<AudioSource> ().PlayOneShot (gameOverSong);
		yield return new WaitForSeconds(1.0f);
		playerBody.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		Time.timeScale = 1f;
		uiGameOver.SetActive (true);
	}
}
