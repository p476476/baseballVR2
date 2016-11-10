using UnityEngine;
using System.Collections;

public class Catcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ball") 
		{
			if (collision.gameObject.GetComponent<Ball> ().state == Ball.State.Flying1)
				catchBall (collision.gameObject.GetComponent<Ball> ());
		}

	}

	void catchBall(Ball ball){
		ball.state = Ball.State.Nohit;
	
	}
}
