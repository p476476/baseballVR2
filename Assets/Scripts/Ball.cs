using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    private Rigidbody rb;


    // 進壘位置
    GameObject target;

    // 狀態
    public enum State : int
    {
        Held,       // 投手準備
        Flying1,    // 投手投出
        Hit,        // 打擊瞬間
        Nohit,      // 沒打到
        Flying2,    // 擊出飛行
        Down,		// 著地
        Catched,     //被接住
        Delete
    };
    public State state;

    // 球種
    public enum Type : int
    {
        Fastball,
        Curve,
        Slider,
        Snake,
        Shadow
    };
    private Type type;
    private float changeSpeed;
    // 水平/縱向偏移
    private Vector3 shotAngle;


    // Flying1飛行時間
    private float arrivalTime;

    //貝茲曲線 讓球彎曲
    Bezier bezier;
    Vector3[] points;// 貝茲曲線 的點 2~4個

    //用於分身球
    bool isUseBallShadows = false;
    GameObject[] ballShadows;

    //預測打出後球落地位置
    public Vector3 drop_place;

    //用於自訂碰撞偵測
    bool haveDoneDetect = false;//是否已偵測完畢
    GameObject bat_top;
    GameObject bat;

    GameObject admin;

    Vector3 hit_position;
    float lifetime = 10;

    void Awake()
    {

        rb = GetComponent<Rigidbody>();

        target = GameObject.Find("target");

        //this.GetComponent<Transform>().localScale = new Vector3(10, 10, 10);
        bezier = new Bezier();

       
        bat = GameObject.FindGameObjectWithTag("Bat");
        bat_top = bat.transform.GetChild(0).gameObject;
        
        

        admin = GameObject.FindGameObjectWithTag("Admin");
    }

    void Start()
    {

        //rb.useGravity = false;

    }


    void FixedUpdate()
    {
        if (state == State.Flying1)
        {

            Move();
            //分身球快到進壘點時會消失
            if (isUseBallShadows == true && Vector3.Distance(target.transform.position, this.transform.position) < 1)
            {
                foreach (var i in ballShadows)
                {
                    Destroy(i);
                }
            }

            //快進壘時做自訂的碰撞偵測
            if (this.transform.position.z - target.transform.position.z < 1)
            {
                //當球與球棒交錯時做偵測
                if (isBallAfterBat(this.transform.position, bat.transform.position, bat_top.transform.position) && !haveDoneDetect)
                {
                    bool result = my_colision_detection(this.transform.position, rb.velocity, bat.transform.position, bat_top.transform.position);
                    if (result == true)//打中
                    {
                        print("HIt IT!!");
                        //算出碰撞點 並把球移到碰撞點
                        this.transform.position = get_colision_position(this.transform.position, rb.velocity, bat.transform.position, bat_top.transform.position);
                        //呼叫 hitTheBall Func
                        GameObject.FindGameObjectWithTag("Bat").GetComponent<Bat>().hitTheBall(this.gameObject);
                    }
                    haveDoneDetect = true;
                }
            }

        }
        if (state == State.Catched)
        {
            Move();
        }

    }

    void Update()
    {

        if (state == State.Hit)
        {
            Hit();
            cal_drop_place();
        }

        lifetime -= Time.deltaTime;

        if (state == State.Delete/*還沒用到*/ || lifetime <= 0)
        {
            Out();
        }

        if (transform.position.z < -90)
        {
            Out();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //成功打擊 
        if (col.gameObject.name == "Bat" && state == State.Flying1)
        {
            col.gameObject.GetComponent<Bat>().hitTheBall(this.gameObject);
            if (changeSpeed - 1.0f < -0.2f || changeSpeed - 1.0f > 0.2f)
                admin.GetComponent<CalPlayerData>().Strike(5);

            admin.GetComponent<CalPlayerData>().Strike((int)type);
            admin.GetComponent<CalPlayerData>().lifeUpdate(true);

            /*test*/
           /* admin.GetComponent<effect>().ShootFireWork();
            admin.GetComponent<Audio>().Cheers();
            admin.GetComponent<Audio>().foul_ball();*/
        }

        //沒打到球
        else if (col.gameObject.name != "Bat" && state == State.Flying1)
        {
          
            admin.GetComponent<CalPlayerData>().lifeUpdate(false);
            state = State.Nohit;
        }
        //打擊後落地
        else if (col.gameObject.name == "Ground" && state == State.Flying2) //如果col碰撞事件的物件名稱是Ground
        {
            //  print("Ball Drop on the ground.");
            state = State.Down;
            //  resultBoard.GetComponent<CalPlayerData>().distanceScore(gameObject.transform);
        }


    }
    void OnTriggerEnter(Collider col)
    {
        //在這裡直接打擊出界外區才當作界外( 與正式棒球不同 )
        if (col.gameObject.tag == "BoundaryLine" && state == State.Flying2)
        {
            admin.GetComponent<CalPlayerData>().Strike(7);          //界外球+1
            admin.GetComponent<Audio>().foul_ball();
        }
        if (col.gameObject.tag == "HomeRun" && state == State.Flying2)
        {
            admin.GetComponent<CalPlayerData>().Strike(6);          //全壘打+1
            
            admin.GetComponent<effect>().ShootFireWork();           //發射煙火
            admin.GetComponent<Audio>().Cheers();                   //觀眾歡呼聲
        }
    }
    //依據球的狀態計算玩家資料 CalPlayerData
    void OnCollisionExit(Collision col)
    {



    }
    public void setShot(float ball_speed, Type ball_type, float change_speed)
    {//球速,球種,變速(進壘時速度是投出時的幾倍 1=不變)

        int tmp;
        float time = (this.transform.position.z - target.transform.position.z) / ball_speed;
        type = ball_type;
        changeSpeed = change_speed;
        //設定不同球種的曲線
        Debug.Log("ball type = " + ball_type);
        if (ball_type == Type.Fastball)
        {
            points = new Vector3[] { this.transform.position, target.transform.position };
        }
        else if (ball_type == Type.Curve)
        {
            Vector3 disp = target.transform.position - transform.position;//起點到終點的位移
            Vector3 p1 = transform.position + Random.Range(0.1f, 0.3f) * disp;//貝茲曲線的中間點
            p1.y += -disp.z * Random.Range(0.1f, 0.2f);
            points = new Vector3[] { this.transform.position, p1, target.transform.position };
        }
        else if (ball_type == Type.Slider)
        {
            Vector3 disp = target.transform.position - transform.position;//起點到終點的位移
            Vector3 p1 = transform.position + Random.Range(0.45f, 0.55f) * disp;//貝茲曲線的中間點
            tmp = Random.Range(0, 2);
            if (tmp == 0) p1.x -= -disp.z * Random.Range(0.1f, 0.2f);
            else if (tmp == 1) p1.x += -disp.z * Random.Range(0.1f, 0.2f);
            p1.y += -disp.z * Random.Range(0.1f, 0.2f);
            points = new Vector3[] { this.transform.position, p1, target.transform.position };
        }
        else if (ball_type == Type.Snake)
        {
            Vector3 disp = target.transform.position - transform.position;//起點到終點的位移
            Vector3 p1 = transform.position + Random.Range(0.15f, 0.35f) * disp;//貝茲曲線的中間點
            Vector3 p2 = transform.position + Random.Range(0.75f, 0.85f) * disp;//貝茲曲線的中間點
            tmp = Random.Range(0, 2);
            if (tmp == 0)
            {
                p1.x -= -disp.z * Random.Range(0.2f, 0.3f);
                p2.x += -disp.z * Random.Range(0.05f, 0.15f);
            }
            else if (tmp == 1)
            {
                p1.x += -disp.z * Random.Range(0.2f, 0.3f);
                p2.x -= -disp.z * Random.Range(0.05f, 0.15f);
            }

            points = new Vector3[] { this.transform.position, p1, p2, target.transform.position };
        }
        else if (ball_type == Type.Shadow)
        {
            CreateShadow();
            isUseBallShadows = true;
            points = new Vector3[] { this.transform.position,
            new Vector3(transform.position.x+Random.Range(-2.0f, 2.0f), transform.position.y +Random.Range(0.0f, 0.6f), transform.position.z-Random.Range(1.0f, 2.0f)),
                target.transform.position };
        }
        bezier.setBezier(points, time);
        //設定變速(變速後時間會不準呦)
        bezier.setSpeed(1f, change_speed);
    }


    void CreateShadow()//分身球
    {
        ballShadows = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            //分身與真正的球的偏移量
            float disp_x = Random.Range(-0.2f, 0.2f);
            float disp_y = Random.Range(-0.2f, 0.2f);
            float disp_z = Random.Range(-0.2f, 0.2f);
            Vector3 pos = new Vector3(this.transform.position.x + disp_x, this.transform.position.y + disp_y, this.transform.position.z + disp_z);
            ballShadows[i] = Instantiate(Resources.Load("ballShadow"), pos, Quaternion.identity) as GameObject;
            ballShadows[i].transform.parent = this.transform;
        }
    }

   /* void OnDrawGizmos()
    {
        if (state == State.Flying1)
        {
            //畫出貝茲曲線的點
            Gizmos.color = Color.magenta;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawSphere(points[i], 0.3f);
            }
        }

        if (state == State.Flying2)
        {

            //畫出落地點
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(drop_place, 0.3f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit_position, 0.3f);
    }
    */
    public void setState(State state)
    {
        this.state = state;

    }


    void Fly()
    {

    }

    void Move()
    {
        if (!bezier.isArrived())
        { // 還沒抵達目標
            bezier.addCurrentTime(Time.fixedDeltaTime);
            Vector3 v = bezier.GetCurrentSpeed();
            rb.velocity = v;
            Vector3 pos = bezier.GetCurrentPosition();
            this.transform.position = pos;
        }

    }

    public void Hit()
    {
    }

    void Out()
    {
        Destroy(gameObject);
    }

    //計算落地點
    public void cal_drop_place()
    {
        Vector3 v = rb.velocity; //初速度
        float height = this.transform.position.y; //初始高度
        float g = 9.80665f; //重力

        float t = (v.y + Mathf.Sqrt(v.y * v.y - 4 * (-g / 2) * (height))) / g; //時間t

        drop_place.x = transform.position.x + v.x * t;
        drop_place.y = 0;
        drop_place.z = transform.position.z + v.z * t;

    }
    //改善碰撞會穿透的問題

    //偵測球是否已經穿過球棒
    bool isBallAfterBat(Vector3 ball_pos, Vector3 bat_pos, Vector3 bat_top_pos)
    {
        Vector3 normal = Vector3.Cross(bat_top_pos - bat_pos, Vector3.up);
        //球棒面
        float d = Vector3.Dot(bat_pos, normal);
        float axbyzc = Vector3.Dot(ball_pos, normal);

        if (axbyzc <= d + 0.4)
            return true;
        else
            return
           false;
    }

    //my碰撞偵測
    bool my_colision_detection(Vector3 ball_pos, Vector3 ball_v, Vector3 bat_pos, Vector3 bat_top)
    {
        //球的位置必須在球棒之間 不然return false
        float lower_x, upper_x;
        if (bat_pos.x < bat_top.x)
        {
            lower_x = bat_pos.x;
            upper_x = bat_top.x;
        }
        else
        {
            lower_x = bat_top.x;
            upper_x = bat_pos.x;
        }
        if (ball_pos.x < lower_x || ball_pos.x > upper_x)
            return false;
        //兩直線
        //球的軌跡(v1,p1) 
        //球棒(v2,p2)

        Vector3 v1 = ball_v;
        Vector3 v2 = bat_top - bat_pos;
        Vector3 p1 = ball_pos;
        Vector3 p2 = bat_pos;

        //求兩直線最短距離
        Vector3 a = p2 - p1;
        Vector3 b = Vector3.Cross(v1, v2);
        Vector3 result = Vector3.Project(a, b);
        float distance = Vector3.Magnitude(result);


        float t1 = Vector3.Dot(Vector3.Cross(a, v1), b) / (Vector3.Magnitude(b) * Vector3.Magnitude(b));

        hit_position = p2 + t1 * v2;

        float t2 = Vector3.Dot(Vector3.Cross(a, v2), b) / (Vector3.Magnitude(b) * Vector3.Magnitude(b));


        print("distance = " + distance + " t1 = " + t1);
        print("Time = " + Time.fixedDeltaTime + " t2 = " + t2);

        if (distance < 0.4 && t1 >= 0 && t1 <= 1 && t2 <= 0 && t2 >= -2f * Time.fixedDeltaTime)
            return true;
        else
            return false;

    }

    //得到碰撞點
    Vector3 get_colision_position(Vector3 ball_pos, Vector3 ball_v, Vector3 bat_pos, Vector3 bat_top)
    {
        //兩直線
        //球的軌跡(v1,p1) 
        //球棒(v2,p2)

        Vector3 v1 = ball_v;
        Vector3 v2 = bat_top - bat_pos;
        Vector3 p1 = ball_pos;
        Vector3 p2 = bat_pos;

        //求兩直線最短距離
        Vector3 a = p2 - p1;
        Vector3 b = Vector3.Cross(v1, v2);
        float t = Vector3.Dot(Vector3.Cross(a, v2), b) / (Vector3.Magnitude(b) * Vector3.Magnitude(b));

        return p1 + t * v1;

    }




}
