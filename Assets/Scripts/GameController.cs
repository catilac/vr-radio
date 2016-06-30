using UnityEngine;
using System.Collections;

public class GameController : MonoSingleton<GameController> {

	private const int kPMLeftControllerDeviceIndex = 1;
	private const int kPMRightControllerDeviceIndex = 2;

	public musicPlayback mp;

	public Camera mainCamera;
	public SimHand simHand;
	public float speedMultiplier = 0.5f;
	private float handSpeedMultiplier = 0.1f;

	//Steam VR Controller variables
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private SteamVR_Controller.Device leftController = SteamVR_Controller.Input (kPMLeftControllerDeviceIndex);
	private SteamVR_Controller.Device rightController = SteamVR_Controller.Input (kPMRightControllerDeviceIndex);
	private SteamVR_TrackedObject trackedObj;

    //For gesture tracking
    public GameObject LeftController;
    public GameObject RightController;

    //For model swapping
    public GameObject leftHandModel;
	public GameObject rightHandModel;
	public GameObject steamLeftControllerModel;
	public GameObject steamRightControllerModel;

	//For hand posing
	public HandAnimator leftHandAnim;
	public HandAnimator rightHandAnim;

	public bool leftGripButtonDown = false;	
	public bool rightGripButtonDown = false;

	public bool leftGripButtonUp = false;		
	public bool rightGripButtonUp = false;

	public bool leftGripButtonPressed = false;	
	public bool rightGripButtonPressed = false;	

	public bool leftTriggerButtonDown = false;		
	public bool leftTriggerButtonUp = false;		
	public bool leftTriggerButtonPressed = false;

	public bool rightTriggerButtonDown = false;		
	public bool rightTriggerButtonUp = false;		
	public bool rightTriggerButtonPressed = false;

	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360, 180);
	public bool lockCursor;
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;

  // Gesture Stuff
	Gesture gesture;

	protected override void Awake() {
		replaceSteamControllerModels (steamLeftControllerModel, leftHandModel, steamRightControllerModel, rightHandModel);		
		base.Awake ();
	}

	void Start() {
		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;

    // Gesture Recognizer
		gesture = new Gesture();
		gesture.player = mp;

		//Steam VR tracked object.
		trackedObj = GetComponent<SteamVR_TrackedObject>();

		//Get references to posing animation objects
		leftHandAnim = leftHandModel.GetComponent<HandAnimator>();
		rightHandAnim = rightHandModel.GetComponent<HandAnimator>();
	}

	public void Update() {
		leftGripButtonDown = leftController.GetPressDown(gripButton);
		leftGripButtonUp = leftController.GetPressUp(gripButton);
		leftGripButtonPressed = leftController.GetPress(gripButton);

		rightGripButtonDown = rightController.GetPressDown(gripButton);
		rightGripButtonUp = rightController.GetPressUp(gripButton);
		rightGripButtonPressed = rightController.GetPress(gripButton);

		leftTriggerButtonDown = leftController.GetPressDown(triggerButton);
		leftTriggerButtonUp = leftController.GetPressUp(triggerButton);
		leftTriggerButtonPressed = leftController.GetPress(triggerButton);

		rightTriggerButtonDown = rightController.GetPressDown(triggerButton);
		rightTriggerButtonUp = rightController.GetPressUp(triggerButton);
		rightTriggerButtonPressed = rightController.GetPress(triggerButton);

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
		if(Input.GetKeyUp("/")) {
			//Hack – swap hand models
			HandAnimator temp = leftHandAnim;
			leftHandAnim = rightHandAnim;
			rightHandAnim = temp;
		}

		if (Input.GetKey ("e")) {
			handUpdate ();
			Vector3 pos = rightHandModel.transform.position;
			gesture.StartGestureRecognition(new Vector2(pos.x, pos.y));
		} else if (Input.GetKeyUp("e")) {
			gesture.StopGestureRecognition();
		}
		else {
			mouseUpdate ();
		}

		/* Hand posing */
		if (leftGripButtonDown) {
			leftHandAnim.setPoint ();
		}
		if (leftTriggerButtonDown) {
			leftHandAnim.setThumbsUp ();
		}
		if (!leftGripButtonPressed && !leftTriggerButtonPressed) {
			leftHandAnim.setIdle ();
		}
		if (rightGripButtonDown) {
			rightHandAnim.setPoint ();
		}
		if (rightTriggerButtonDown) {
			rightHandAnim.setThumbsUp ();
		}
		if (!rightGripButtonPressed && !rightTriggerButtonPressed) {
			rightHandAnim.setIdle ();
		}

		if (leftGripButtonPressed) {
			Vector3 pos = LeftController.transform.position;
			gesture.StartGestureRecognition (new Vector2 (pos.x, pos.y));
		} else if (leftGripButtonUp) {
			gesture.StopGestureRecognition ();
		}

		if (rightGripButtonPressed) {
			Vector3 pos = RightController.transform.position;
			gesture.StartGestureRecognition (new Vector2 (pos.x, pos.y));
		} else if (rightGripButtonUp) {
			gesture.StopGestureRecognition ();
		}

			
			

		if (leftController == null || rightController == null) {
			Debug.Log("Controller not initialized");
			return;
		}


		logControllerInteraction (leftController);
		logControllerInteraction (rightController);
	}

	private void move(Vector3 direction, Transform entity) {
		Vector3 pos = entity.position;
		pos += direction * speedMultiplier;
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

	private void logControllerInteraction(SteamVR_Controller.Device controller){
		if (leftGripButtonDown) {			        
			Debug.Log("Grip Button was just pressed");
		}
		if (leftGripButtonUp) {
			Debug.Log("Grip Button was just unpressed");
		}
		if (leftTriggerButtonDown){
			Debug.Log("Trigger Button was just pressed");
		}
		if (leftTriggerButtonUp) {
			Debug.Log("Trigger Button was just unpressed");
		}
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

		rightHandModel.transform.position += mainCamera.transform.up * mouseDelta.y * handSpeedMultiplier + 
			mainCamera.transform.right * mouseDelta.x * handSpeedMultiplier;
	}

	private void replaceSteamControllerModels(GameObject oldLeft, GameObject newLeft, GameObject oldRight, GameObject newRight) {
		Transform leftParent = oldLeft.transform.parent;
		Transform rightParent = oldRight.transform.parent;
		newLeft.transform.SetParent (leftParent, false);
		newRight.transform.SetParent (rightParent, false);
		oldLeft.SetActive (false);
		oldRight.SetActive (false);
		HandAnimator leftAnimator = leftHandModel.GetComponent<HandAnimator>();
		HandAnimator rightAnimator = rightHandModel.GetComponent<HandAnimator>();
		leftAnimator.parent = leftParent.gameObject;
		rightAnimator.parent = rightParent.gameObject;
	}
}
