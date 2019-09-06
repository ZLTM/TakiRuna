using UnityEngine;
using System.Collections;

public class DeadZoneController : MonoBehaviour {

	public GameObject GM;
	public AudioClip audioClip;

	void OnTriggerEnter (Collider other) {
		if (GM.GetComponent<GMController>().isDying) {
			return;
		}
		if (other.CompareTag("Enemy")) {
			GM.GetComponent<GMController> ().gameOver ();
			GetComponent<AudioSource>().PlayOneShot(audioClip);
		}
	}
}
