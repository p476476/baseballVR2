using UnityEngine;
using System.Collections;

public class TT : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	
	void FixedUpdate() {

		transform.RotateAround(transform.position, -transform.up, Time.deltaTime * 150f);

		/*
		Quaternion rotation = Quaternion.LookRotation(this.transform.right*300);
		transform.rotation = rotation;*/


	}






}
