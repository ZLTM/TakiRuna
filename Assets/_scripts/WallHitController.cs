using UnityEngine;
using System.Collections;

public class WallHitController : MonoBehaviour {

	// Wall number
	//Top 0 - Right 1 - Bottom 2 - Left 3
	public GameObject GM;
	public GameObject ritualObj;
	public AudioClip thisAudio;
	public int thisWall = 0;

	void OnTriggerEnter (Collider other) {
		if (GM.GetComponent<GMController>().getDidRitual ()) {
			return;
		}
		if (other.CompareTag("Myo")) {
			bool canHit = ritualObj.GetComponent<RitualController>().canHitWalls;
			bool onTime = GM.GetComponent<GMController> ().getOnTime ();
			if (thisWall == 7) {
				ritualObj.GetComponent<RitualController>().canHitWalls = true;
				return;
			}
			if (canHit && onTime) {
				transform.GetChild(0).GetComponent<ParticleSystem> ().Play();
				GetComponent<AudioSource> ().PlayOneShot (thisAudio);
				ritualObj.GetComponent<RitualController> ().canHitWalls = false;
				ritualObj.GetComponent<RitualController> ().ritualMov (thisWall);
			} else if (canHit && !onTime) {
				ritualObj.GetComponent<RitualController> ().ritualMov (77);
			}
		}

	}
}
