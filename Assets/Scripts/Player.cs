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
		if (controller.GripButtonDown) {
			isHoldingBat = !isHoldingBat;
		}
		if (isHoldingBat) {

			bat.transform.SetParent (controller.gameObject.transform);
		} else {

			bat.transform.SetParent (null);
		}
		



	}

}
