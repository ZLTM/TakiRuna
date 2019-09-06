using UnityEngine;
using System.Collections;

public class CCController : MonoBehaviour {


	void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Finish")) {
			Destroy (this.gameObject);
		}
	}
}
