using UnityEngine;
using System.Collections;

public class SimHand : MonoBehaviour {

	public Camera mainCamera;
	private float distance = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
		transform.rotation = new Quaternion (0.0f, mainCamera.transform.rotation.y, 0.0f, mainCamera.transform.rotation.w);
	}

}
