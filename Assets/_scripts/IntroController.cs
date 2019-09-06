using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour {

	public GameObject allAnim;
	public GameObject uiCnt;
	public GameObject uiThudner;
	public Text uiText;
	public GameObject Thunder;
	public GameObject trigoT;
	public GameObject trigoAll;
	public GameObject deadC;

	private int originalAlpha;
	private Color originalColor;
	private Color originalColorT;
	private float trigoDuration;

	void Start () {
		originalAlpha = 150;
		originalColor = uiCnt.GetComponent<Image> ().color;
		originalColorT = uiThudner.GetComponent<Image> ().color;
		trigoDuration = 1.0f;
		hideText (0f);
		StartCoroutine(playIntro());
		StartCoroutine(throwT());
		Time.timeScale = 0.75f;
	}
	
	IEnumerator playIntro() {
		yield return new WaitForSeconds(1);
		showText ("Once upon a time there was a humble native...");
		yield return new WaitForSeconds(4);
		hideText (1f);
		yield return new WaitForSeconds(1);
		showText ("He was happy with his simple life.\n"+"But he did't know that he was a little bit special...\n"+"He was the chonse to defend Pachamama");
		yield return new WaitForSeconds(2.5f);
		trigoAll.SetActive (true);
		yield return new WaitForSeconds(2.5f);
		hideText (0.1f);
		Thunder.GetComponent<ParticleSystem> ().Play ();
		Thunder.GetComponent<AudioSource> ().Play ();
		StartCoroutine(thunder ());
	}

	void showText(string text) {
		uiText.text = text;
		StartCoroutine(showCnt (0.5f));
	}

	void hideText(float fadeTime) {
		uiText.text = "";
		StartCoroutine(hideCnt (fadeTime));
	}

	IEnumerator hideCnt(float fadeTime) {
		float currentTime = 0.0f;
		do {
			uiCnt.GetComponent<Image> ().color = new Color (originalColor.r,originalColor.g,originalColor.b,
															originalColor.a- originalColor.a*currentTime/fadeTime);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= fadeTime);
		uiCnt.GetComponent<Image> ().color = originalColor;
		uiCnt.SetActive (false);
	}

	IEnumerator showCnt(float fadeTime) {
		float currentTime = 0.0f;
		uiCnt.SetActive (true);
		do {
			uiCnt.GetComponent<Image> ().color = new Color (originalColor.r,originalColor.g,originalColor.b,originalColor.a*currentTime/fadeTime);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= fadeTime);
	}


	IEnumerator throwT() {
		yield return new WaitForSeconds(2f);
		trigoT.SetActive (true);
		yield return new WaitForSeconds(trigoDuration);
		trigoT.SetActive (false);
		//3
		yield return new WaitForSeconds(0.2f);
		//3.2
		trigoT.SetActive (true);
		yield return new WaitForSeconds(trigoDuration);
		trigoT.SetActive (false);
		//4.2
		yield return new WaitForSeconds(2.4f);
		//6.8
		trigoT.SetActive (true);
		yield return new WaitForSeconds(trigoDuration);
		//7.8
		yield return new WaitForSeconds(3.5f);
		trigoT.SetActive (false);
		//11.3

		StartCoroutine(throwT());
	}

	IEnumerator thunder() {
		float currentTime = 0.0f;
		float fadeTime = 0.5f;
		Time.timeScale = 1f;
		uiThudner.SetActive (true);
		GetComponent<AudioSource> ().Stop ();
		do {
			uiThudner.GetComponent<Image> ().color = new Color (originalColorT.r,originalColorT.g,originalColorT.b,originalColorT.a*currentTime/fadeTime);
			currentTime += Time.deltaTime;
			yield return null;
		} while (currentTime <= fadeTime);

		deadC.SetActive (true);
		uiThudner.SetActive (false);
		Destroy (allAnim.gameObject);
		StopAllCoroutines ();
		StartCoroutine(nextScene());
	}

	IEnumerator nextScene() {
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel ("01-menu");
	}
}
