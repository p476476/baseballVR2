using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	private Rigidbody rb;

	public GameObject target;

	private Vector3 toV;
	private int power = 60;

	private float t;

	private Vector3 biasHorizontal = new Vector3(250,0,0);


	void Start () {

		rb = this.gameObject.GetComponent<Rigidbody>();
		rb.useGravity = false;

		toV = target.transform.position - this.transform.position;
		toV.Normalize();



		rb.AddForce(biasHorizontal);


		rb.velocity = toV*power;




		Debug.Log (rb.velocity.z.ToString());
		Debug.Log (Time.fixedDeltaTime);

		t = 90/rb.velocity.z;
		Debug.Log (t);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){



		Vector3 v = new Vector3 (500 * Time.fixedDeltaTime / t, 0, 0);
		rb.AddForce(v);

	}
}
