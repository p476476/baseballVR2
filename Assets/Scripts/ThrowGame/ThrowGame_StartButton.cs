using UnityEngine;
using System.Collections;

public class ThrowGame_StartButton : MonoBehaviour {

    string name = "Strat";
    float timer;
    float active_time = 3;
    bool enable = false;
    bool isTouched = false;

    //光環
    public ParticleSystem cycle_paritcle;
    Color cycle_color;

    // Use this for initialization
    void Start () {
        timer = 0;

        cycle_color = new Color(0.0f,0.2f,1f);
    }
	
	// Update is called once per frame
	void Update () {

        //觸碰時光環會變色
        cycle_color.r = Mathf.Lerp(0, 1, timer / active_time);
        cycle_color.b = Mathf.Lerp(0, 1, 1 - timer / active_time);
        cycle_paritcle.gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", cycle_color);


        if (isTouched&&timer< active_time)
            timer += Time.deltaTime;
        else if(timer>0)
            timer -= Time.deltaTime;

    }
   void OnTriggerEnter()
    {
        isTouched = true;
    }

    void OnTriggerExit()
    {
        isTouched = false;
    }

}
