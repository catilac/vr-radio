using UnityEngine;
using System.Collections;

public class BoxMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("w")) {
			Vector3 pos = transform.position;
			pos.x += 0.5f;
			transform.position = pos;

		}
	}
}
