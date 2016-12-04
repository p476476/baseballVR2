using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class strike_game_btn : MonoBehaviour {

    public ControllerBase ctrl;
    public string txt;

    Color cycle_col;
	// Use this for initialization
	void Start () {
        cycle_col = new Color(0.0f, 0.2f, 1f);
    }
    void OnTriggerEnter()
    {
        this.GetComponent<AudioSource>().Play();
    }
    void OnTriggerStay(Collider col)
    {
        cycle_col.r = 1.0f;
        if (col.tag == "bat" && ctrl.TriggerButtonDown)
        {
            if(txt == "start")
            {
                CalPlayerData.Instance.init();
                Manager.instance.gamestart = true;                      //可以投球
                this.gameObject.transform.parent.gameObject.SetActive(false);                //將button關掉
            }
            else if(txt == "home")
            {
                SceneManager.LoadScene("menu");
            }
        }
    }

    void OnTriggerExit()
    {
        cycle_col.r = 0.0f;
    }

}
