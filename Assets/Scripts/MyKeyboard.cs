using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyKeyboard : MonoBehaviour {

	public MyKeyboard Inctance;

	public Text txt_input;

	bool isCaps = true;

	void Awake(){
		Inctance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void inputChar(char ch){
		txt_input.text += ch;
	}

	public void pressEnter()
	{
		
	}

	public void pressCaps()
	{
		isCaps = !isCaps;
	}
}
