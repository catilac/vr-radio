using UnityEngine;
using System.Collections;

public class HandAnimator : MonoBehaviour {

	private Animator anim;

	public void Start() {
		anim = GetComponent<Animator> ();
	}
	public bool idleHand;
	public bool thumbHand;
	public bool pointHand;

	public void setIdle() {
		idleHand = true;
		thumbHand = false;
		pointHand = false;
		updateParams ();
	}

	public void setThumbsUp() {
		idleHand = false;
		thumbHand = true;
		pointHand = false;
		updateParams ();
	}

	public void setPoint() {
		idleHand = false;
		thumbHand = false;
		pointHand = true;
		updateParams ();
	}

//Enable if you want to debug
//	public void Update() {
//		if (Input.GetKeyDown ("b")) {
//			idleHand = true;
//			thumbHand = false;
//			pointHand = false;
//			updateParams ();
//		}
//		if (Input.GetKeyDown ("n")) {
//			idleHand = false;
//			thumbHand = true;
//			pointHand = false;
//			updateParams ();
//		}
//		if (Input.GetKeyDown ("m")) {
//			idleHand = false;
//			thumbHand = false;
//			pointHand = true;
//			updateParams ();
//		}
//	}

	private void updateParams() {
		anim.SetBool ("idleHand", idleHand);
		anim.SetBool ("thumbHand", thumbHand);
		anim.SetBool ("pointHand", pointHand);
	}
}
