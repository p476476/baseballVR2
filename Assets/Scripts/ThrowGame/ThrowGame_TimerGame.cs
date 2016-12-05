using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrowGame_TimerGame : MonoBehaviour {

    public static ThrowGame_TimerGame Instance;

    //遊戲文字
    public TextMesh txt_string_score;
    public TextMesh txt_string_leftTime;

    //遊戲參數文字
    public TextMesh txt_score;
    public TextMesh txt_leftTime;
    public TextMesh txt_Debug;
    public GameObject message;

    //數字牌
    NumberColtroller number_ctlr;
    public GameObject numbers;
    Vector3 shelf_start_pos;//九宮格起始位置
    int min_num = -5; //最小數字
    int max_num = 20; //最大數字

    //遊戲參數
    int score;
    float left_time;
    int level; //關卡
    int game_time = 10;//遊戲時間

    //連擊
    int combo_count=1;
    bool isComboing; //正在連擊
    float init_combo_timer = 0.5f;
    float combo_timer;


    //數字版計時器
    //數到0時重新產生新版子
    public float[] num_timer;

    //可以獲得球?
    bool canGetBall = false;


    int temp_score; //做分數慢慢加的效果

    //特效
    public ParticleSystem scoreStar;

    //能量表
    public ThrowGame_EnergyBar energyBar;

    /*====================system functions======================*/

    void Awake()
    {
        Instance = this;
    }

    void Start () {
        //get number controller
        number_ctlr = numbers.GetComponent<NumberColtroller>();

        //儲存九宮格起始位置
        shelf_start_pos = new Vector3();
        shelf_start_pos = number_ctlr.transform.position;

        //宣告計時器
        num_timer = new float[10];

        this.gameObject.SetActive(false);

        
    }
	
	void Update () {
        //做分數慢慢加的效果
        if (temp_score < score)
            temp_score += (score - temp_score) / 30 + 1;
        else if (temp_score > score)
            temp_score += (score - temp_score) / 30 - 1;


        //遊戲中
        if (ThrowGameManager.Instance.gameState == ThrowGameManager.StateType.PLAYING)
        {
            //更新時間
            left_time -= Time.deltaTime;
            for (int i = 1; i < 10; i++)
            {
                num_timer[i] -= Time.deltaTime;
                if (num_timer[i] < 0)
                {
					// 計時器數到0 產生Number
                    number_ctlr.destoryNumber(i, 0);
                    number_ctlr.createNumber(i, Random.Range(min_num, max_num));
                    num_timer[i] = 10;
                }
            }

            //更新文字
            update_text();

            //連擊計時
            if (combo_timer > 0)
                combo_timer -= Time.deltaTime;
            else if(isComboing)//連擊時間結束
            {
                if(combo_count >= 2)//有達成連擊
                    message.GetComponent<ThrowGame_Message>().show(combo_count + "連擊!!");
                isComboing = false; 
                combo_count = 1;
            }


            //當遊戲時間結束
            if (left_time < 0)
            {
                StartCoroutine(gameOver());
            }
        }
    }

    //===================My Functions===================//

    //開始此遊戲
    public IEnumerator startGame()
    {
        //初始化參數
        combo_count = 1;
        score = 0;
        temp_score = 0;
        number_ctlr.transform.position = shelf_start_pos;

        //初始計時器
        left_time = game_time;
        for (int i = 1; i < 10; i++)
            num_timer[i] = 10;


        //清除所有數字牌子
        number_ctlr.destoryAllNumber(0);

        //開啟文字顯示
        enableAllText();
        update_text();

        //分數為白色(因為倒數時會變紅 怕沒改回來)
        txt_leftTime.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        //開啟能量表
        //energyBar.gameObject.SetActive(true);
       // energyBar.init();

        //倒數
        ThrowGame_Message msg = message.GetComponent<ThrowGame_Message>();
        msg.show("READY");
        yield return new WaitForSeconds(1.6f);
		//播放倒數音效
		ThrowGame_MusicManager.Instance.playCountDownSound();
        msg.show("3");
        yield return new WaitForSeconds(1.0f);
        msg.show("2");
        yield return new WaitForSeconds(1.0f);
        msg.show("1");
        yield return new WaitForSeconds(1.0f);
        msg.show("START");

        //產生數字牌
        for(int i = 1; i < 10; i++)
        {     
            number_ctlr.createNumber(i, Random.Range(min_num, max_num));
        }

        //更改遊戲狀態為 PLAYING
        ThrowGameManager.Instance.gameState = ThrowGameManager.StateType.PLAYING;
    }

    //打到牌子
    public void hitNumber(number num) 
    {
        //計算連擊
        if (!isComboing)
        {
            isComboing = true;
            combo_count = 1;
            combo_timer = init_combo_timer;
        }
        else
        {
            combo_count++;
        }


        //加分
        scoreStar.Emit(5);
        score += (int)(num.myNum*(combo_count+1)*0.5);     
       

        //加能量
        //energyBar.addEnergy((float)num.myNum);

        //1秒後刪除被打中的牌子
        number_ctlr.destoryNumber(num.myPos, 1.0f);
        //2秒後產生新牌子
        num_timer[num.myPos] = 2f;
    }

	IEnumerator gameOver()
    {
        ThrowGameManager.Instance.gameState = ThrowGameManager.StateType.GAME_END;
        message.GetComponent<ThrowGame_Message>().show("遊戲結束");
		yield return new WaitForSeconds(2.0f);

		number_ctlr.destoryAllNumber(0);

        //倒數文字變回白色
        txt_leftTime.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        disableAllText ();

        //關閉能量表
        //energyBar.gameObject.SetActive(false);

        

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

            ThrowGame_ScoreRecorder.Instance.addScore(record, ThrowGameManager.Mode.TIMER_MODE);
        }


        ThrowGameManager.Instance.backToUnstart();

        this.gameObject.SetActive(false);
    }


    //開啟所有文字顯示
    void enableAllText()
    {
        //遊戲文字
        txt_string_score.gameObject.GetComponent<MeshRenderer>().enabled = true;
        txt_string_leftTime.gameObject.GetComponent<MeshRenderer>().enabled = true;

        //遊戲參數文字
        txt_score.gameObject.GetComponent<MeshRenderer>().enabled = true;
        txt_leftTime.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    //關閉所有文字顯示
    void disableAllText()
    {
        //遊戲文字
        //txt_string_score.gameObject.GetComponent<MeshRenderer>().enabled = false;
        txt_string_leftTime.gameObject.GetComponent<MeshRenderer>().enabled = false;

        //遊戲參數文字
        //txt_score.gameObject.GetComponent<MeshRenderer>().enabled = false;
        txt_leftTime.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    //更新文字
    void update_text()
    {

		if (left_time < 10.5f)
			txt_leftTime.gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
            
        txt_score.text = temp_score.ToString("0000");
        txt_leftTime.text = "" + left_time.ToString("00");

    }

    public bool useOneBall()
    {
        if (left_time > 0)
            return true;
        else
            return false;
    }

    //=================Unity Event=====================//
}
