using UnityEngine;
using System.Collections;

public class RhythmController : MonoBehaviour {

	public GameObject GM;
	public GameObject ritualObj;
	public GameObject beatCircle;

	public float soundDuration;
	public float soundDelay;

	void Start () {
		StartCoroutine(playRhythm());
	}

	IEnumerator playRhythm() {
		GM.GetComponent<GMController>().setDidRitual (false);
		GM.GetComponent<GMController>().setOnTime (true);
		yield return new WaitForSeconds(soundDuration);
		GM.GetComponent<GMController>().setOnTime (false);
		if (!GM.GetComponent<GMController>().getDidRitual()) {
			ritualObj.GetComponent<RitualController> ().sequenceFailed ();
		}
		GM.GetComponent<GMController>().setDidRitual (false);
		beatCircle.GetComponent<BeatController> ().startBeat (soundDuration+soundDelay);
		yield return new WaitForSeconds(soundDelay);
		if (!GM.GetComponent<GMController>().isDying) {
			StartCoroutine(playRhythm());
		}
	}
		
}
