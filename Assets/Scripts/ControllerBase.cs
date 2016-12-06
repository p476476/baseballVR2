using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ControllerBase : MonoBehaviour {

	// 板機, 握住, 中心觸碰板、選單
	private Valve.VR.EVRButtonId TriggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId GripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId TouchpadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
	private Valve.VR.EVRButtonId MenuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId testbutton = Valve.VR.EVRButtonId.k_EButton_System;


	//壓下的瞬間、彈起的瞬間、持續壓住
	public bool TriggerButtonDown = false;
	public bool TriggerButtonUp = false;
	public bool TriggerButtonPressed = false;


	public bool GripButtonDown = false;
	public bool GripButtonUp = false;
	public bool GripButtonPressed = false;

	public bool TouchpadButtonDown = false;
	public bool TouchpadButtonUp = false;
	public bool TouchpadButtonPressed = false;

	public bool MenuButtonDown = false;

	public bool testbuttonDown = false;

	private SteamVR_Controller.Device controller {get { return SteamVR_Controller.Input ((int)trackedObj.index);}}
	private SteamVR_TrackedObject trackedObj;

	public bool modeTouch = true;
	public bool modeLaser = true;
	public bool modePadClick = true;
	public bool modePadSlide = true;
	public bool modeMovement = true;

	public GameObject touchObject;

	private float rayLength = 100;
	public LayerMask wallLayer;
	private RaycastHit hit;
	private LineRenderer lineRenderer;

	public Vector3 v;




	void Start () {

		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		lineRenderer = this.GetComponent<LineRenderer>();
	}

	void Update () {

		if (controller == null) {
			Debug.Log ("Controller null");
			return;
		}

		getButtonsState ();







	}

	void FixedUpdate(){
		v = controller.velocity;
	}

	private void getButtonsState(){

		TriggerButtonDown = controller.GetPressDown (TriggerButton);
		TriggerButtonUp = controller.GetPressUp (TriggerButton);
		TriggerButtonPressed = controller.GetPress (TriggerButton);

		GripButtonDown = controller.GetPressDown (GripButton);
		GripButtonUp = controller.GetPressUp (GripButton);
		GripButtonPressed = controller.GetPress (GripButton);

		TouchpadButtonDown = controller.GetPressDown (TouchpadButton);
		TouchpadButtonUp = controller.GetPressUp (TouchpadButton);
		TouchpadButtonPressed = controller.GetPress (TouchpadButton);

		MenuButtonDown = controller.GetPressDown (MenuButton);

		testbuttonDown = controller.GetPressDown (testbutton);
	}

	public Vector2 getPadDirection(){
		return controller.GetAxis();
	}

	public int getPad4Direction (){
		Vector2 pos = controller.GetAxis();
		if (pos.y > 0.5) {return 1;	}
		if (pos.y < -0.5) { return 2; }

		if (pos.x > 0.5) {return 4;	}
		if (pos.x < -0.5) { return 3; }

		return 0;
	}

	private void LaserSet(bool state){

		lineRenderer.SetPosition(0,trackedObj.transform.position);
		if (state) {
			lineRenderer.SetPosition (1, trackedObj.transform.position + trackedObj.transform.forward * 50);
		} else {
			lineRenderer.SetPosition(1,trackedObj.transform.position);
		}
	}

	private void LaserDetect (){

		Vector3 fwd = trackedObj.transform.TransformDirection(Vector3.forward);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, fwd, out hit, rayLength, wallLayer))
		{
			if (hit.collider != null) {

				Debug.Log (hit.collider.name);
			}
		}
	}

	public void Shock(ushort i){

		controller.TriggerHapticPulse (i);


	}
}
