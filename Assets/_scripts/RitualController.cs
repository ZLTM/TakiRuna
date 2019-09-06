using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using VibrationType = Thalmic.Myo.VibrationType;

public class RitualController : MonoBehaviour {

	public GameObject GM;
	public GameObject waveMaster;
	public GameObject ritualCnt;
	public GameObject ritualMovImg;
	public bool canHitWalls = true;
	public Sprite[] ritualImages;

	public GameObject myo = null;
	private ThalmicMyo thalmicMyo;

	private bool newSequence;
	private int[] correctSequence;
	private int sequenceIndex;

	private int [][] allSequences;
	private int[] sequence0 = new int[] {1,3};
	private int[] sequence1 = new int[] {3,1};
	private int[] sequence2 = new int[] {0,2};

	private int[] sequence3 = new int[] {1,3,0};
	private int[] sequence4 = new int[] {2,0,1};
	private int[] sequence5 = new int[] {0,1,2};

	private int[] sequence6 = new int[] {0,1,2,3};
	private int[] sequence7 = new int[] {1,3,0,2};
	private int[] sequence8 = new int[] {1,3,3,1};

	private int[] sequence9 = new int[] {1,2,3,0,3};
	private int[] sequence10 = new int[] {0,2,2,1,3};
	private int[] sequence11 = new int[] {1,3,1,0,2};


	private int[] sequence12 = new int[] {1,2,3,0,3,1,3,1,0,2};
	private int[] sequence13 = new int[] {0,2,2,1,3,1,2,3,0,3};
	private int[] sequence14 = new int[] {1,3,1,0,2,0,1,2,3,2};


	void Start () {
		allSequences = new int[][]{sequence0, sequence1, sequence2,
			sequence3, sequence4, sequence5,
			sequence6, sequence7, sequence8,
			sequence9, sequence10, sequence11,
			sequence12, sequence13, sequence14};
		
		correctSequence = allSequences[0];
		newSequence = true;
		sequenceIndex = 0;

		thalmicMyo = myo.GetComponent<ThalmicMyo> ();
	}
	
	public void ritualMov (int wallHit) {
		GM.GetComponent<GMController>().setDidRitual (true);
		if (newSequence) {
			sequenceIndex = 0;
		}
		if (correctSequence[sequenceIndex] == wallHit) {
			checkRitualImg ();
			newSequence = false;
			sequenceIndex += 1;
			if (correctSequence.Length == sequenceIndex) {
				waveMaster.GetComponent<WaveController>().ritualComplete();
				newSequence = true;
				sequenceIndex = 0;
			}
		} else {
			sequenceFailed();
		}
	}

	public void setCorrectSequence(int sequence) {
		int i;
		Sprite arrow;
		GameObject newRitualMovImg;
		Debug.Log ("Sequence "+sequence);
		correctSequence = allSequences[sequence];
		foreach (Transform child in ritualCnt.transform) {
			GameObject.Destroy (child.gameObject);
		}
		for (i=0;i<correctSequence.Length;i++) {
			newRitualMovImg = Instantiate (ritualMovImg) as GameObject;
			newRitualMovImg.transform.SetParent (ritualCnt.transform);
			newRitualMovImg.transform.localScale = Vector3.one;
			arrow = ritualImages [correctSequence [i]];
			newRitualMovImg.GetComponent<Image> ().sprite = arrow;
		}
	}

	public void sequenceFailed() {
		newSequence = true;
		sequenceIndex = 0;
		if (!newSequence) {
			thalmicMyo.Vibrate (VibrationType.Medium);
			Debug.Log ("FAAAAILLL");
		}
		clearRitualImg ();
	}

	void checkRitualImg() {
		GameObject currentRitualImg;
		currentRitualImg = ritualCnt.transform.GetChild (sequenceIndex).gameObject;
		currentRitualImg.GetComponent<Image> ().color = Color.cyan;
	}

	void clearRitualImg() {
		foreach (Transform child in ritualCnt.transform) {
			child.GetComponent<Image> ().color = Color.white;
		}
	}
}
