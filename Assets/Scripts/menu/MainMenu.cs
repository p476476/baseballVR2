using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    public void StrikeMode()
    {
        SceneManager.LoadScene("main");
    }
    public void throw_mode()
    {
        SceneManager.LoadScene("practice");
    }
    public void exit()
    {
        Application.Quit();    //需確認用法
    }
}
