using UnityEngine;
using System.Collections;

public class BeatController : MonoBehaviour {

	public GameObject centerCircle;
	public int time;

	private Vector3 originalScale;
	private Vector3 destinationScale;

	void Start () {
		originalScale = transform.localScale;
		destinationScale = centerCircle.transform.localScale;
	}
	

	public void startBeat(float time) {
		StartCoroutine (beat(time));
	}

	IEnumerator beat(float time) {

		float currentTime = 0.0f;

		do {
			transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= time);
		transform.localScale=originalScale;
	}
}
