using UnityEngine;
using System.Collections;

public class ThrowGame_Message : MonoBehaviour {

    Animator anim;
    int textShow = Animator.StringToHash("Base Layer.textShow");

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void show(string str)
    {
        GetComponent<TextMesh>().text = str;
        anim.SetTrigger("show");
    }

    public void keepShow(string str)
    {
        GetComponent<TextMesh>().text = str;
        anim.Play("Idle");
    }
}
