using UnityEngine;
using System.Collections;

public class TestRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	float current;
	float x=1;
	// Update is called once per frame
	void Update () {
		
		this.transform.Rotate (Vector3.up, x*Time.deltaTime*60);
		current+=x*Time.deltaTime*60;
		if (current > 180 || current < 0)
			x *= -1;

	}
}
