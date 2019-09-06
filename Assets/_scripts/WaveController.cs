using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveController : MonoBehaviour {

	public GameObject rhythmObj;
	public GameObject ritualObj;
	public GameObject waveInstances;
	public GameObject playerPart;
	public AudioClip audioLvlUp;
	public AudioClip audioCCosaDead;
	public Text uiLvlText;
	public Text uiWavesText;

	private float[] lvlTimes;
	private int currentLvl;
	private float cycleTime;
	private int randomSequence;
	private int wavesClear;
	private int totalWavesCleared;
	private bool isBoss;

	void Start () {
		lvlTimes = new float[]{0.5f, 0.5f,	0.45f, 0.4f,	0.35f, 0.3f,	0.3f, 0.2f};
		currentLvl = -1;
		randomSequence = 0;
		wavesClear = 0;
		isBoss = false;

		StartCoroutine(initialWave());
	}


	void Update () {
	
	}

	void lvlUp () {
		currentLvl += 1;
		uiLvlText.text = "Level: "+currentLvl;
		if (currentLvl==3) {
			uiLvlText.text = "MAX LEVEL!";
		}
		transform.GetComponent<AudioSource> ().PlayOneShot (audioLvlUp);
		cycleTime = lvlTimes [currentLvl] + lvlTimes [currentLvl + 1];
		rhythmObj.GetComponent<RhythmController> ().soundDuration = lvlTimes[currentLvl*2];
		rhythmObj.GetComponent<RhythmController> ().soundDelay = lvlTimes[currentLvl*2 +1];
	}

	public void ritualComplete() {
		Debug.Log("RITUAL COMPLETE!");
		killWave ();
		wavesClear += 1;
		totalWavesCleared += 1;
		uiWavesText.text = "Waves Cleared: "+totalWavesCleared;
		if (isBoss) {
			wavesClear = 0;
			isBoss = false;
			if (currentLvl < 3) {
				lvlUp ();
			} else if (currentLvl ==1) {
				playerPart.transform.Find ("FirePower").GetComponent<ParticleSystem> ().Play();
			}
		}
		if (wavesClear == 4) {
			spawnBoss ();
		} else {
			spawnChavicosas ();
		}
	}
		
	void spawnChavicosas() {
		//GetComponent<AudioSource>().Play();
		randomSequence = Random.Range(currentLvl, (currentLvl+1)*3);
		ritualObj.GetComponent<RitualController> ().setCorrectSequence (randomSequence);
		transform.GetChild(currentLvl).GetComponent<SpawnController>().spawnChavicosas(2f*cycleTime*currentLvl+3);
		Debug.Log ("New Wave");
	}

	void spawnBoss () {
		//GetComponent<AudioSource>().Play();
		isBoss = true;
		randomSequence = Random.Range(12, 15);
		ritualObj.GetComponent<RitualController>().setCorrectSequence(randomSequence);
		transform.GetChild(currentLvl).GetComponent<SpawnController>().spawnBoss(1.5f*cycleTime*10);
		Debug.Log ("New BOSS!!");
	}

	void killWave() {
		foreach (Transform child in waveInstances.transform) {
			foreach (Transform childAnim in child) {
				childAnim.GetChild (0).gameObject.SetActive (true);
			}
			child.GetComponent<Rigidbody> ().useGravity = true;
		}
		playerPart.transform.Find ("FireExp").GetComponent<ParticleSystem> ().Play();
		transform.GetComponent<AudioSource> ().PlayOneShot (audioCCosaDead);
	}

	IEnumerator initialWave() {
		yield return new WaitForSeconds(3f);
		lvlUp ();
		spawnChavicosas ();
	}
}
