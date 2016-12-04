using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public ControllerBase controller;
	public GameObject bat;

    

	public bool isHoldingBat = false;

	enum State {Preparatory,Ready,Running,Safety};

	void Start () {

        
    }

	void Update () {
		/*if (controller.GripButtonDown) {
			isHoldingBat = !isHoldingBat;
		}*/
        if (controller.isActiveAndEnabled)
        {
            bat.transform.SetParent(controller.transform);
            bat.transform.localPosition = new Vector3(0.02f, -0.01f, -0.067f);
        }
            
	}

}
