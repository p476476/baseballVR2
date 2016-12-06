using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrowGame_NormalGame : MonoBehaviour {
    public static ThrowGame_NormalGame Instance;

    //遊戲文字
    public TextMesh txt_string_score;
    public TextMesh txt_string_goal;
    public TextMesh txt_string_leftBall;

    //遊戲參數文字
    public TextMesh txt_score;
    public TextMesh txt_goal;
    public TextMesh txt_leftBall;
    public TextMesh txt_Debug;
    public GameObject message;

    //數字牌
    NumberColtroller number_ctlr;
    public GameObject numbers;
    Vector3 shelf_start_pos;//九宮格起始位置


    //遊戲參數
    int score;
    int left_ball;
    int goal_number;
    int combo_number; //連擊
    int level; //關卡
	int init_left_ball = 15;//起始剩餘球數
	public int count_ball_can_drop;//剩餘幾球可若地(當=0且沒打完時遊戲結束)


    List<int> numList; //紀錄還有哪些數字牌
    int temp_score; //做分數慢慢加的效果

    //特效
    public ParticleSystem scoreStar;

    //障礙
    public GameObject obstacle;




	//=====================System Function====================//

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    public void Start () {

        //get number controller
        number_ctlr = numbers.GetComponent<NumberColtroller>();
        numList = new List<int>();

        shelf_start_pos = new Vector3();
        shelf_start_pos = number_ctlr.transform.position;

        this.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        //做分數慢慢加的效果
        if (temp_score < score)
            temp_score+=(score-temp_score)/30+1;
        else if (temp_score > score)
            temp_score += (score - temp_score) / 30-1;

        update_text();
    }

	//=====================My Function====================//

	//要開始遊戲請呼叫這個function
    public IEnumerator startGame()
    {

        //init
        combo_number = 1;
        score = 0;
        temp_score = 0;
		left_ball = init_left_ball;
		count_ball_can_drop = init_left_ball;
        number_ctlr.transform.position = shelf_start_pos;

        numList.Clear();
        for (int i = 1; i < 10; i++)
            numList.Add(i);

        level = 1;

        //開啟文字(目標和剩餘球數)
        enableAllText();

        //顯示 LEVEL1 和 START
        message.GetComponent<ThrowGame_Message>().show("LEVEL 1");
        yield return new WaitForSeconds (1.6f);
        message.GetComponent<ThrowGame_Message>().show("START");

        ThrowGameManager.Instance.gameState = ThrowGameManager.StateType.PLAYING;

        makeNumsAndGoal(); //產生數字牌和目標
    }


	//產生數字牌和目標
    void makeNumsAndGoal()
    {
        //clean numbers
        number_ctlr.init();

        //create numbers
        for (int i = 1; i < 10; i++)
        {
            number_ctlr.createNumber(i,i);
        }

        //set goal
        goal_number = numList[Random.Range(0, numList.Count - 1)];
        number_ctlr.setGoal(goal_number);
    }

    //下一關 和 關卡設置
    IEnumerator level_up()
    {
        //level+1
        level++;

        //輸出 clear
        message.GetComponent<ThrowGame_Message>().show("CLEAR");
        yield return new WaitForSeconds(3f);

        //清除數字牌
        number_ctlr.init();
        numList.Clear();
        for (int i = 1; i < 10; i++)
            numList.Add(i);

        //重製可用球數
		left_ball = init_left_ball;
		count_ball_can_drop = init_left_ball;

        //關卡設置
        switch (level)
        {
            case 2:

                //增加距離
                number_ctlr.transform.Translate(new Vector3(0, 0, 2));
                message.GetComponent<ThrowGame_Message>().show("LEVEL 2");
                yield return new WaitForSeconds(1.6f);
                message.GetComponent<ThrowGame_Message>().show("START");
                makeNumsAndGoal(); //產生數字牌和目標
                break;
            case 3:

                //增加距離
                number_ctlr.transform.Translate(new Vector3(0, 0, 1));

                message.GetComponent<ThrowGame_Message>().show("LEVEL 3");
                yield return new WaitForSeconds(1.6f);
                message.GetComponent<ThrowGame_Message>().show("START");

                //增加障礙
                obstacle.GetComponent<ThrowGame_Obstacle>().enableObstacle();

                makeNumsAndGoal(); //產生數字牌和目標
                break;
            case 4:
                StartCoroutine(gameClear());
                break;
        }
    }

	//球落地時 Ball呼叫此function
	public void ballDropOnGround()
	{
		count_ball_can_drop--;
        update_text();
        //print (count_ball_can_drop + " " + numList.Count);
		//還有數字沒打完
		if (count_ball_can_drop<=0 && numList.Count!=0 && left_ball<=0) {
			StartCoroutine(gameOver());
		}
	}

	//遊戲結束
	IEnumerator gameOver()
    {
		print ("game OvER");
        ThrowGameManager.Instance.gameState = ThrowGameManager.StateType.GAME_END;
        //移除障礙
        obstacle.GetComponent<ThrowGame_Obstacle>().disableObstacle();

        


        message.GetComponent<ThrowGame_Message>().show("GAME OVER");
		yield return new WaitForSeconds(2.0f);

        disableAllText();


        ThrowGameManager.Instance.backToUnstart();
        this.gameObject.SetActive(false);
		
        
    }


	//遊戲過關
	IEnumerator gameClear()
    {
        ThrowGameManager.Instance.gameState = ThrowGameManager.StateType.GAME_END;
        //移除障礙
        obstacle.GetComponent<ThrowGame_Obstacle>().disableObstacle();

        message.GetComponent<ThrowGame_Message>().show("恭喜過關!!");
        yield return new WaitForSeconds(1.5f); 

        //分數進排行榜
        if (ThrowGame_ScoreRecorder.Instance.isHighScore(score))
        {
            message.GetComponent<ThrowGame_Message>().show("恭喜上榜");
            yield return new WaitForSeconds(1.5f);
            message.GetComponent<ThrowGame_Message>().keepShow("輸入你的名字");


            //開啟鍵盤
            MyKeyboard.Instance.enableKeyboard();
            

            //等到案ENTER
            while (!MyKeyboard.Instance.finishInput())
                yield return new WaitForSeconds(0.5f);

            //關閉鍵盤
            MyKeyboard.Instance.disableKeyboard();

            //更新到排行榜
            string player_name = MyKeyboard.Instance.txt_input.text.ToString();
            if (name == "")
                name = "[    ]";

            score_tuple record = new score_tuple();
            record.score = this.score;
            record.name = player_name;
            record.date = "2016";

            ThrowGame_ScoreRecorder.Instance.addScore(record, ThrowGameManager.Mode.NORMAL_MODE);
        }

        

        disableAllText();
        ThrowGameManager.Instance.backToUnstart();
        this.gameObject.SetActive(false);
        

    }
    
	//沒有打到牌子且球落地時
    public void missHit() 
    {
       // txt_Debug.text = "you miss !";
        combo_number = 1;
    }

	//打到牌子時
    public void hitNumber(int num) 
    {
        //加減分
        if (num == goal_number)
        {
            //播放星星特效
            scoreStar.Emit(5);
            score += combo_number* combo_number*15;
            combo_number ++;
        }
        else {
            score -= 5;
            combo_number = 1;
        }

        //移除數字
        numList.Remove(num);
        print("num left :"+ numList.Count);

        //若數字打完 下一關
        if (numList.Count <= 0)
        {
            StartCoroutine(level_up());

        }

        //更新目標
        if (numList.Count>0 && num == goal_number)
        {
            goal_number = numList[Random.Range(0, numList.Count - 1)];
            number_ctlr.setGoal(goal_number);
        }

    }

	//手呼叫此function 要一顆球
    public bool useOneBall()
    {
        if (left_ball <= 0)
        {
            return false;
        }
        else
        {
            left_ball--;
            return true;
        }
        
    }

	//更新遊戲文字
    void update_text()
    {
        txt_score.text = temp_score.ToString("0000");
        txt_goal.text = goal_number.ToString();
        txt_leftBall.text = "" + left_ball;
        
        if (numList != null)
        {
            string str = "";
            foreach (var i in numList)
            {
                str += i.ToString();
            }
            txt_Debug.text = str;
        }
        
    }

    void enableAllText()
    {
        //遊戲文字
        txt_string_score.gameObject.GetComponent<MeshRenderer>().enabled = true;
        txt_string_goal.gameObject.GetComponent<MeshRenderer>().enabled = true;
        txt_string_leftBall.gameObject.GetComponent<MeshRenderer>().enabled = true;

        //遊戲參數文字
        txt_score.gameObject.GetComponent<MeshRenderer>().enabled = true;
        txt_goal.gameObject.GetComponent<MeshRenderer>().enabled = true;
        txt_leftBall.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    void disableAllText()
    {
        //遊戲文字
        //txt_string_score.gameObject.GetComponent<MeshRenderer>().enabled = false;
        txt_string_goal.gameObject.GetComponent<MeshRenderer>().enabled = false;
        txt_string_leftBall.gameObject.GetComponent<MeshRenderer>().enabled = false;

        //遊戲參數文字
        //txt_score.gameObject.GetComponent<MeshRenderer>().enabled = false;
        txt_goal.gameObject.GetComponent<MeshRenderer>().enabled = false;
        txt_leftBall.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
