using UnityEngine;
using System.Collections;

public class ResultBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.name == "Ground")
			GetComponent<Rigidbody>().isKinematic = true;
		
	}

}
