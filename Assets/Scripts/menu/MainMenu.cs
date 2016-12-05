using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

	// Use this for initialization

    

	void Start () {
       StartCoroutine(loadScenes());
    }

    IEnumerator loadScenes()
    {

        
        yield return SceneManager.CreateScene("main");

        yield return SceneManager.CreateScene("practice");


    }
	
	// Update is called once per frame
    public void StrikeMode()
    {
        //StartCoroutine(loadScene("main"));
        loadScene("main");
    }
    public void throw_mode()
    {
        //SceneManager.l
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("practice"));
        //SceneManager.LoadScene("practice");
        //StartCoroutine(loadScene("practice"));
        loadScene("practice");
    }


    void loadScene(string str)
    {

       SceneManager.LoadSceneAsync(str);
    }

    public void exit()
    {
        Application.Quit();    //需確認用法
    }
}
