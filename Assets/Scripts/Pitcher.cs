using UnityEngine;
using System.Collections;

public class Pitcher : MonoBehaviour
{

    public ControllerBase controller;
    // 進壘位置
    public GameObject target;

    // 投手面對方向
    public GameObject PitcherToward;

    // 將投的球
    private GameObject heldBall;

    //球數與關卡
    private int ballnum;
    private int level;

    // 球初速與變速比例
    public float initialSpeed = 10;
    public float changeSpeed = 1;

    //要投的球的種類
    private Ball.Type balltype;
    
    //動畫
    public Animator anim;
    AnimationState animState;
    int anim_throw = Animator.StringToHash("Base Layer.Throw_1");
    int anim_idle = Animator.StringToHash("Base Layer.Idle");

    private bool ControlActtive;
    //起始位置
    public Vector3 start_pos;
    //右手位置
    public Transform trans_righthand;

    void Start()
    {
        this.transform.LookAt(PitcherToward.transform);
        start_pos = new Vector3();
        start_pos = this.transform.position;
        ballnum = 0;
        ControlActtive = false;
    }

    void Update()
    {
        if (controller.TouchpadButtonPressed)
        {
            moveTarget();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            balltype = Ball.Type.Fastball;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            balltype = Ball.Type.Curve;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            balltype = Ball.Type.Slider;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            balltype = Ball.Type.Snake;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            balltype = Ball.Type.Shadow;
        }
        else if (Input.GetKeyDown(KeyCode.A))
            ControlActtive = !ControlActtive;

        //動畫控制
        AnimatorStateInfo asi = anim.GetCurrentAnimatorStateInfo(0);

        if (asi.IsName("Throw_1"))
        {
            anim.SetBool("throw_1", false);
            Manager.instance.IsbackPos = false;
        }
        else if (asi.IsName("Idle 0"))
        {
            Manager.instance.IsbackPos = true;
        }
    }

    void moveTarget()
    {
        Vector2 padMove = controller.getPadDirection() * 0.01f;
        target.transform.Translate(padMove.x, padMove.y, 0);
    }

    public void throwBall()
    {
        anim.SetBool("throw_1", true);
    }

    // 持球 - 產生球物件
    public void Holding()
    {

        heldBall = Instantiate(Resources.Load("ball"), trans_righthand.position, Quaternion.identity) as GameObject;
        heldBall.GetComponent<Ball>().setState(Ball.State.Held);
        //heldBall.transform.Translate(,Space.World);

        Pitching();
    }

    float SpeedMin = 10.0f, SpeedMax = 15.0f, changeMin = 0.3f, changeMax;
    int tmp;
    // 投球 - 設定球的方向與出力
    void Pitching()
    {

        level = ballnum / 10;
        if (!ControlActtive)
        {
            target.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            //前五個level，每個levle一種球路；其餘球路隨機，5、6 level 球速逐漸增加；7、8變速；
            if (level < 5)
            {
                initialSpeed = Random.Range(SpeedMin, SpeedMax);
                balltype = (Ball.Type)level;
            }
            else
            {
                balltype = (Ball.Type)Random.Range(0, 5);
                if (level < 7)
                    initialSpeed = Random.Range(++SpeedMin, ++SpeedMax);
                else if (level < 9)
                {
                    tmp = Random.Range(0, 2);
                    if (tmp == 0)
                    {
                        initialSpeed = Random.Range(10.0f, 20.0f);
                        changeSpeed = Random.Range(1.5f, 3.0f);
                    }
                    else
                    {
                        initialSpeed = Random.Range(40.0f, 60.0f);
                        changeSpeed = Random.Range(0.05f, 0.3f);
                    }
                }
                else
                {
                    initialSpeed = Random.Range(++SpeedMin, ++SpeedMax);
                    tmp = Random.Range(0, 10);
                    if (tmp >= 2)
                    {
                        if (changeSpeed <= 0.8)
                            changeMin += 0.02f;
                        changeSpeed = Random.Range(changeMin, 1.0f);
                    }
                    else
                    {
                        changeSpeed = 0.01f;
                    }
                }
            }
        }
        heldBall.GetComponent<Ball>().setShot(initialSpeed, balltype, changeSpeed);
        // heldBall.GetComponent<Ball>().setShot(60, balltype, 3.0f);
        heldBall.GetComponent<Ball>().setState(Ball.State.Flying1);
        ballnum++;
    }

    public void back_to_start_position()
    {
        this.transform.position = start_pos;
        this.transform.LookAt(PitcherToward.transform);
    }

}
