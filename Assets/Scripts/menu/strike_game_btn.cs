using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class strike_game_btn : MonoBehaviour {

    public ControllerBase ctrl;
    public string txt;
    public ParticleSystem particle;
    Color cycle_col;
	// Use this for initialization
	void Start () {
        cycle_col = new Color(0.0f, 0.2f, 1f);
        particle.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", cycle_col);
    }
    void OnTriggerEnter()
    {
        this.GetComponent<AudioSource>().Play();
        cycle_col.r = 1.0f;
        particle.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", cycle_col);
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Bat" && ctrl.TriggerButtonDown)
        {
            //for test
            if(txt == "start")
            {
                CalPlayerData.Instance.init();                     //初始化玩家資料
                Pitcher.Instance.ballnum = 0;                       // 球數初始化
                Manager.instance.GameStart();                      //可以投球
                gameObject.transform.parent.gameObject.SetActive(false);                //將button關掉
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
        particle.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", cycle_col);
    }

}
