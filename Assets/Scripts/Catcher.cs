using UnityEngine;
using System.Collections;

public class Catcher : MonoBehaviour {

    GameObject admin;
    // Use this for initialization
    void Awake() { 
        admin = GameObject.FindGameObjectWithTag("Admin");
    }
	
	// Update is called once per frame

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Ball") 
		{
            if (collision.gameObject.GetComponent<Ball>().state == Ball.State.Flying1)
            {
                admin.GetComponent<CalPlayerData>().lifeUpdate(false);
              
            }
                
		}
	}
}
