using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public GameObject waveInstances;
	public GameObject cCosa;
	public GameObject superCCosa;
	public int spawnNumber = 20;
	public int spawnBossNumber = 10;

	private float distanceToC;

	void Start () {
		distanceToC = transform.position.sqrMagnitude;
	}

	public void spawnChavicosas(float lifeTime) {
		int i;
		int cCosaAnim = Random.Range(0, 3);
		GameObject thisCCosa;
		GameObject cCosaChild;
		Quaternion rotation;
		Vector3 newPos;
		float thisAngle;
		float spawnVel = distanceToC / lifeTime+1;

		for (i=0;i<spawnNumber;i++) {
			thisAngle = 360/spawnNumber * i;
			rotation = Quaternion.identity;
			rotation.eulerAngles = new Vector3(0,thisAngle+180,0);
			newPos = Quaternion.Euler (0, thisAngle, 0) * transform.position;
			thisCCosa = Instantiate (cCosa, newPos, rotation) as GameObject;
			cCosaChild = thisCCosa.transform.GetChild (cCosaAnim).gameObject;
			cCosaChild.SetActive (true);
			cCosaChild.transform.localRotation = Quaternion.Euler (0,180* Random.Range(0,2),0);
			thisCCosa.transform.SetParent (waveInstances.transform);
			thisCCosa.GetComponent<Rigidbody> ().velocity = -thisCCosa.transform.localPosition.normalized * spawnVel*Time.deltaTime;
		}
	}

	public void spawnBoss(float lifeTime) {
		int i;
		GameObject thisSuperCCosa;
		Quaternion rotation;
		GameObject cCosaChild;
		Vector3 newPos;
		float thisAngle;
		float spawnVel = distanceToC / lifeTime+1;

		for (i=0;i<spawnBossNumber;i++) {
			thisAngle = 360/spawnBossNumber * i;
			rotation = Quaternion.identity;
			rotation.eulerAngles = new Vector3(0,thisAngle+180,0);
			newPos = Quaternion.Euler (0, thisAngle, 0) * transform.position;
			thisSuperCCosa = Instantiate (superCCosa, newPos, rotation) as GameObject;
			cCosaChild = thisSuperCCosa.transform.GetChild (0).gameObject;
			cCosaChild.SetActive (true);
			cCosaChild.transform.localRotation = Quaternion.Euler (0,180* Random.Range(0,2),0);
			thisSuperCCosa.transform.SetParent (waveInstances.transform);
			thisSuperCCosa.GetComponent<Rigidbody> ().velocity = thisSuperCCosa.transform.forward * spawnVel*Time.deltaTime;
		}

	}
}
