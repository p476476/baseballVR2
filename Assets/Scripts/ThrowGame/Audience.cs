using UnityEngine;
using System.Collections;

public class Audience : MonoBehaviour {

    Animation anim;

	// Use this for initialization
	void Start () {
        StartCoroutine(random_anim());
    }

    IEnumerator random_anim()
    {
        GetComponent<Animator>().speed = Random.Range(0, 2000);

        yield return new WaitForSeconds(0.1f);

        GetComponent<Animator>().speed = 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
