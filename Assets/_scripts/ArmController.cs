using UnityEngine;
using System.Collections;

public class ArmController : MonoBehaviour {

	public GameObject playerArm;

	void Update () {
		transform.rotation = playerArm.transform.rotation;
	}
}
