using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

	public Animator anim;
	ControllerBase ctrl;
	int holding = Animator.StringToHash("Base Layer.Holding");
	int unhold = Animator.StringToHash("Base Layer.Unhold");

    public GameObject light_ball;


	public bool is_holding_thing = false;
	GameObject holded_obj;

	//Rigidbody hand_rb;
	public Transform hand_center;

	public float hand_power = 1.5f;
	Vector3 hand_speed;
	Vector3 old_pos;

    //手正在碰到的物體
    List<GameObject> colliders_list;

    //
    public bool canUseLightingBall = false;


    //震動幅度 
    ushort min_shock_value = 100;
    ushort max_shock_value = 300;
    ushort shock_value = 0;

    //======================System Function========================//
    void Start () {

        //得到ControllerBase
        ctrl = GetComponentInParent<ControllerBase> ();
		old_pos = this.transform.position;

        colliders_list = new List<GameObject>();

    }
	
	void Update () {

		//得到當前手的移動速度
		hand_speed = (this.transform.position - old_pos) / Time.deltaTime;
		old_pos = this.transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(transform.up * 0.03f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(transform.up * -0.03f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(transform.right * -0.03f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(transform.right * 0.03f);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            this.transform.Translate(transform.forward * 0.03f);
        }
        if (Input.GetKey(KeyCode.X))
        {
            this.transform.Translate(transform.forward * -0.03f);
        }

        if(anim.GetBool("hold")&&is_holding_thing == true)
        {
            ctrl.Shock(shock_value);

            shock_value += (ushort)(100 * Time.deltaTime);
            shock_value = (ushort)Mathf.Clamp(shock_value, min_shock_value, max_shock_value);
        }
            

        //按下版機時 手彎曲 獲得球
        if (ctrl.TriggerButtonDown==true)
        {
            //撥放手彎曲的動畫
            anim.SetBool("hold", true);

            


            //找碰撞體中第一個按鈕
            foreach (var obj in colliders_list)
            {
                
                if (obj != null&&obj.active ==true)
                {
                    print(obj.tag);
                    if (obj.tag == "Button")
                    {
                        //按下按鈕
                        obj.GetComponent<ThrowGame_Button>().press();
                        //不要繼續執行之後的程式(產生球)
                        return;
                    }
                }
            }


            //產生球
            if (canGetBall())
            {
                canUseLightingBall = true;

                //震動初始化
                shock_value = min_shock_value;

                if (canUseLightingBall)
                {
                    holded_obj = (GameObject)Instantiate(light_ball, hand_center.position, Quaternion.identity) as GameObject;
                    //EventManager.TriggerEvent("consume_energy");
                    canUseLightingBall = false;
                }
                else
                    holded_obj = (GameObject)Instantiate(Resources.Load("ball_test"), hand_center.position, Quaternion.identity) as GameObject;

                //球的parent設為手 (跟著手移動)
                holded_obj.transform.parent = this.transform;

                //手拿著物體
                is_holding_thing = true;
            }
        }


        

        //放開版機時 手張開 若握著物體 則將物體丟出
		if (Input.GetKeyUp(KeyCode.U)||ctrl.TriggerButtonUp==true)
        {
            
            unholdObject();
        }
    }

    //======================My Function========================//

    //是否可以獲得球
    bool canGetBall(){

        if (is_holding_thing)
            return false;

        switch (ThrowGameManager.Instance.gameState)
        {
            case ThrowGameManager.StateType.UNSTART:
                return true;

            case ThrowGameManager.StateType.START_GAME:
                return false;

            case ThrowGameManager.StateType.READY_TO_PLAY:
                return false;

            case ThrowGameManager.StateType.PLAYING:
                if(ThrowGameManager.Instance.mode==ThrowGameManager.Mode.NORMAL_MODE)
                {
                   return ThrowGame_NormalGame.Instance.useOneBall();
                }else if (ThrowGameManager.Instance.mode == ThrowGameManager.Mode.TIMER_MODE)
                {
                    return ThrowGame_TimerGame.Instance.useOneBall();
                }
                break;

            case ThrowGameManager.StateType.PAUSE:
                return false;
            case ThrowGameManager.StateType.GAME_END:
                return false;

            default:
                return false;
        }
        return false;
	}

    //放開物體
    public void unholdObject()
    {
        print("unhold");
        anim.SetBool("hold", false);
        if (is_holding_thing)
        {
            EventManager.TriggerEvent("stopBiggerBall");
           
            holded_obj.transform.parent = null;
            Rigidbody ball_rb = holded_obj.gameObject.GetComponent<Rigidbody>();
    
            ball_rb.useGravity = true;
            ball_rb.velocity = hand_speed*hand_power;

            holded_obj.GetComponent<SphereCollider>().enabled = true;
        }

        is_holding_thing = false;
    }

    void OnTriggerEnter(Collider col)
    {
        colliders_list.Add(col.gameObject);
    }

    void OnTriggerExit(Collider col)
    {
        colliders_list.Remove(col.gameObject);
    }

    //臨時解法
    //因為 button disable 時不會觸發 OnTriggerExit
    //手還以為有在Trigger button
    public void buttonIsDisable(ThrowGame_Button[] btns)
    {
        foreach (var obj in btns)
        {
            colliders_list.Remove(obj.gameObject);
        }
    }

    //===============Unity Event===============//
    void OnEnable()
    {
        EventManager.StartListening("energy", getEnergyBall);
    }

    void OnDisable()
    {
        EventManager.StopListening("energy", getEnergyBall);
    }

    void getEnergyBall()
    {
        canUseLightingBall = true;
        
    }

}
