using UnityEngine;
using System.Collections;

public class ThrowGame_Button : MonoBehaviour {

    
    public string txt;

    //計時被碰到多久
    float timer;
    //最大3秒
    float active_time = 3;

    //是否可以被按
    public bool isenable = true;
    bool isTouched = false;

    //光環
    public ParticleSystem cycle_paritcle;
    Color cycle_color;

    //=====================System Function========================//
    void Start()
    {
        timer = 0;

        cycle_color = new Color(0.0f, 0.2f, 1f);
    }


    void Update()
    {

        //觸碰時光環會變色
        cycle_color.r = Mathf.Lerp(0, 1, timer / active_time);
        cycle_color.b = Mathf.Lerp(0, 1, 1 - timer / active_time);
        cycle_paritcle.gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", cycle_color);

        //計算被碰到多久
        if (isTouched && timer < active_time)
            timer += Time.deltaTime;
        else if (timer > 0)
            timer -= Time.deltaTime;

    }


    void OnTriggerStay()
    {
        isTouched = true;
    }

    void OnTriggerExit()
    {
        isTouched = false;
    }

    //=====================My Function========================//

    public void press()
    {
        
		if (isenable)
        {
            GetComponent<AudioSource>().Play();
            isenable = false;
            print("Button " + txt + " is pressed.");

            ThrowGameManager.Instance.pressButton(txt);
        }
        
    }

    public void setEnable(bool e)
    {
		isenable = e;
    }



}
