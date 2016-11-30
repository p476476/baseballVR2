using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	public void Strike()
    {
        audio.PlayOneShot(Resources.Load<AudioClip>("strike"));
    }
}
