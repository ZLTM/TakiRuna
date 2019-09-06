using UnityEngine;
using System.Collections;

public class RotMouseController : MonoBehaviour {


	void Update () {
		transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0)*5);
	}
}
