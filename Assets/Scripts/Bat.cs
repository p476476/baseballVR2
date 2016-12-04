using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour
{

    // 棒頭
    public GameObject top;
    public ControllerBase controller;


    private GameObject admin;
    private Rigidbody rb;
    private Vector3 lastPos;
    private Vector3 lastVelocity = new Vector3(0, 0, 0);
    private Vector3 lastAcceleration = new Vector3(0, 0, 0);

    private float posz = 0.27f;
    private Vector3 velocity = new Vector3(0, 0, 0);
    private Vector3 acceleration = new Vector3(0, 0, 0);

    // 打擊力量
    private float power = 2f;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        admin = GameObject.FindGameObjectWithTag("Admin");

        lastPos = top.transform.position;
    }
    /*  void Update()
      {
          if (Input.GetKey("a"))
              posz += 0.1f;
          if (Input.GetKey("d"))
              posz -= 0.1f;
          gameObject.transform.position = new Vector3(-2.94f, 0.09f, posz);
      }*/
    void FixedUpdate()
    {

        //for test (suould delete)
        if (Input.GetKey(KeyCode.J))//打擊
        {
            Vector3 p = GameObject.Find("target").transform.position;
            p.x -= 2.7f;
            p.z -= 0.2f;
            p.y -= 0.2f;
            transform.position = p;
        }

        if (Input.GetKey(KeyCode.H))//打擊
        {

            this.transform.Rotate(Vector3.down * Time.deltaTime * 800);

        }
        calculateMovement();

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log ("in "+col.gameObject.GetComponent<Rigidbody> ().velocity);

        if (collision.gameObject.tag == "Ball")
        {
           // admin.GetComponent<Audio>().Strike();
            
        }
        // print(rb.velocity.magnitude);
        //這裡想使用移動幅度(速度) 區分長短打
        /*if(rb.velocity.magnitude > 2)*/



    }

    //狀態改變、計算落地點、給予速度
    public void hitTheBall(GameObject ball)
    {
        //controller.Shock ();  //震動
        ball.GetComponent<Ball>().setState(Ball.State.Hit);
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        Vector3 v = constantOfMomentum(ballRb);
        //v.y += 5;
        ballRb.velocity = v;
        ball.GetComponent<Ball>().setState(Ball.State.Flying2);
        ball.GetComponent<Ball>().cal_drop_place();
        //ballRb.velocity = new Vector3(0, 15,15);
    }

    void OnCollisionStay(Collision collision)
    {

    }

    void OnCollisionExit(Collision collision)
    {
        print("OnCollisionExit");


    }

    // 算棒子v,a
    void calculateMovement()
    {

        velocity = top.transform.position - lastPos;
        lastPos = top.transform.position;

        acceleration = velocity - lastVelocity;
        lastVelocity = velocity;

        velocity /= Time.fixedDeltaTime;
        acceleration /= Time.fixedDeltaTime;
    }

    // 動量守恆算擊出速度
    Vector3 constantOfMomentum(Rigidbody ballRb)
    {

        Vector3 sumV = ((ballRb.velocity * ballRb.mass) + (velocity * power * rb.mass)) / (ballRb.mass + rb.mass);
        return sumV;
    }



}
