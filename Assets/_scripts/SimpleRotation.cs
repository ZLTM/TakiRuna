using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour {

	public float rotationAmount = 3;
	public float inX = 0f;
	public float inY = 0f;
	public float inZ = 0f;

	void Update () {
		float rotation = rotationAmount * Time.deltaTime;
		transform.Rotate (rotation*inX,rotation*inY,rotation*inZ);
	}
}
