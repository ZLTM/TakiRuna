using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	public GameObject uiCnt;
	public GameObject bG;

	void Start () {
		StartCoroutine(wait());
	}
	
	IEnumerator wait() {
		yield return new WaitForSeconds(9f);
		uiCnt.SetActive (false);
		bG.SetActive (true);
	}

	public void play() {
		Application.LoadLevel ("02-lvl01");
	}
}
