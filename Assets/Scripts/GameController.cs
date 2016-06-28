using UnityEngine;
using System.Collections;

public class GameController : MonoSingleton<GameController> {

	public Camera mainCamera;
	public float speedMultiplier = 0.5f;

	public void Update() {
		if (Input.GetKey ("w")) {
			move (mainCamera.transform.forward, mainCamera.transform);
		}
		if (Input.GetKey ("s")) {
			move (-mainCamera.transform.forward, mainCamera.transform);
		}
		if (Input.GetKey ("a")) {
			move (-mainCamera.transform.right, mainCamera.transform);
		}
		if (Input.GetKey ("d")) {
			move (mainCamera.transform.right, mainCamera.transform);
		}
	}

	private void move(Vector3 direction, Transform entity) {
		Vector3 pos = entity.position;
		pos += direction * speedMultiplier;
		entity.position = pos;
	}
}
