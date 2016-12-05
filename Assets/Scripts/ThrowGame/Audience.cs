using UnityEngine;
using System.Collections;

public class Audience : MonoBehaviour {

    Animation anim;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().Play("Idle 0", -1, Random.Range(0f, 1f));

    }


	
	// Update is called once per frame
	void Update () {
	
	}

    //===============Unity Event===============//
    void OnEnable()
    {
        EventManager.StartListening("StartGame", celebrating);
        EventManager.StartListening("EndGame", idling);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartGame", celebrating);
        EventManager.StopListening("EndGame", idling);
    }

    void celebrating()
    {
        GetComponent<Animator>().SetTrigger("toCelebrate");
        //StartCoroutine(random_anim());
    }

    void idling()
    {
        GetComponent<Animator>().Play("Idle 0", -1, Random.Range(0f, 1f));
    }
}
