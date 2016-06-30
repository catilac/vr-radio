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

	public void Update() {
		if (Input.GetKeyDown ("b")) {
			idleHand = true;
			thumbHand = false;
			pointHand = false;
			updateParams ();
		}
		if (Input.GetKeyDown ("n")) {
			idleHand = false;
			thumbHand = true;
			pointHand = false;
			updateParams ();
		}
		if (Input.GetKeyDown ("m")) {
			idleHand = false;
			thumbHand = false;
			pointHand = true;
			updateParams ();
		}
	}

	private void updateParams() {
		anim.SetBool ("idleHand", idleHand);
		anim.SetBool ("thumbHand", thumbHand);
		anim.SetBool ("pointHand", pointHand);
	}
}
