using UnityEngine;
using System.Collections;

public class GameController : MonoSingleton<GameController> {

	public Camera mainCamera;
	public SimHand simHand;
	public float speedMultiplier = 0.0005f;
	private float handSpeedMultiplier = 0.1f;

	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360, 180);
	public bool lockCursor;
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;

	public GameObject leftHandModel;
	public GameObject rightHandModel;
	public GameObject steamLeftController;
	public GameObject steamRightController;

	ArrayList pointArr;
	static GameObject gestureDrawing;
	GestureTemplates m_Templates;
	Gesture gesture;

	protected override void Awake() {
		replaceSteamControllerModels (steamLeftController, leftHandModel, steamRightController, rightHandModel);		
		base.Awake ();
	}

	void Start() {
		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;
		gesture = new Gesture();
	}

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

		if (Input.GetKey ("e")) {
			handUpdate ();
			gesture.StartGestureRecognition(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		} else if (Input.GetKeyUp("e")) {
			gesture.StopGestureRecognition();
		}
		else {
			mouseUpdate ();
		}
	}

	private void move(Vector3 direction, Transform entity) {
		Vector3 pos = entity.position;
		pos += direction * speedMultiplier;
		Debug.Log ("move: " + direction * speedMultiplier);
		entity.position = pos;
	}

	private void updateMouseAbsPos(){
		// Get raw mouse input for a cleaner reading on more sensitive mice.
		var mouseDelta = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

		// Scale input against the sensitivity setting and multiply that against the smoothing value.
		mouseDelta = Vector2.Scale (mouseDelta, new Vector2 (sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

		// Interpolate mouse movement over time to apply smoothing delta.
		_smoothMouse.x = Mathf.Lerp (_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
		_smoothMouse.y = Mathf.Lerp (_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

		// Find the absolute mouse movement value from point zero.
		_mouseAbsolute += _smoothMouse;
	}

	private void mouseUpdate(){
		// Ensure the cursor is always locked when set
		Screen.lockCursor = lockCursor;

		// Allow the script to clamp based on a desired target value.
		var targetOrientation = Quaternion.Euler(targetDirection);

		updateMouseAbsPos ();

		// Clamp and apply the local x value first, so as not to be affected by world transforms.
		if (clampInDegrees.x < 360)
			_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

		var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
		mainCamera.transform.localRotation = xRotation;

		// Then clamp and apply the global y value.
		if (clampInDegrees.y < 360)
			_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

		mainCamera.transform.localRotation *= targetOrientation;

		var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, mainCamera.transform.InverseTransformDirection(Vector3.up));
		mainCamera.transform.localRotation *= yRotation;
	}

	private void handUpdate() {
		var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		simHand.transform.position += mainCamera.transform.up * mouseDelta.y * handSpeedMultiplier + 
			mainCamera.transform.right * mouseDelta.x * handSpeedMultiplier;
	}

	private void replaceSteamControllerModels(GameObject oldLeft, GameObject newLeft, GameObject oldRight, GameObject newRight) {
		Transform leftParent = oldLeft.transform.parent;
		Transform rightParent = oldRight.transform.parent;
		newLeft.transform.SetParent (leftParent, false);
		newRight.transform.SetParent (rightParent, false);
		oldLeft.SetActive (false);
		oldRight.SetActive (false);
	}
}
