using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class ThrowGameManager : MonoBehaviour {
    public static ThrowGameManager Instance;

    //按鈕
    public ThrowGame_Button btn_Start;
    public ThrowGame_Button btn_TimerMode;
    public ThrowGame_Button btn_NormalMode;

    //Message
    public ThrowGame_Message message;

    //音樂控制
    public ThrowGame_MusicManager music_mgr;
        

    public Mode mode; //遊戲模式
    public enum Mode
    {
        TIMER_MODE,      //時間制
        NORMAL_MODE     //一般制
    }

    public StateType gameState;
    public enum StateType//遊戲狀態
    {
        UNSTART,
        START_GAME,//按下開始遊戲的瞬間
        READY_TO_PLAY,//按下開始遊戲 到 出現START 之間
        PLAYING,
        PAUSE,
        GAME_END
    }

    void Awake()
    {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
        init();/*
        ThrowGame_NormalGame.Instance.gameObject.SetActive(false);
        ThrowGame_TimerGame.Instance.gameObject.SetActive(false);*/
    }

    void init()
    {
        mode = Mode.NORMAL_MODE;
        gameState = StateType.UNSTART;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mode = Mode.NORMAL_MODE;
            gameState = StateType.START_GAME;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            mode = Mode.TIMER_MODE;
            gameState = StateType.START_GAME;

        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            ThrowGame_NormalGame.Instance.gameObject.SetActive(false);
            ThrowGame_TimerGame.Instance.gameObject.SetActive(false);
        }

        switch (gameState)
        {
            //未開始遊戲
            case StateType.UNSTART:
                break;

            //按下開始遊戲的瞬間
            case StateType.START_GAME:
                disableAllButton();
                gameState = StateType.READY_TO_PLAY;
                music_mgr.playMusic();
                StartCoroutine(startGame());
                break;

            //按下開始遊戲 到 出現START 之間
            case StateType.READY_TO_PLAY:
                break;

            case StateType.PLAYING:
                break;

            case StateType.PAUSE:
                break;

			case StateType.GAME_END:
				endGame ();
	            break;
        }

        
    }

    public IEnumerator startGame()
    {
        switch(mode)
        {
            case Mode.NORMAL_MODE:
                //開啟一般賽管理器
                ThrowGame_NormalGame.Instance.gameObject.SetActive(true);
                //等1秒確保管理器以開啟
                yield return new WaitForSeconds(1.0f);
                //開啟一般賽
                StartCoroutine(ThrowGame_NormalGame.Instance.startGame());
                break;
            case Mode.TIMER_MODE:
                //開啟時間賽管理器
                ThrowGame_TimerGame.Instance.gameObject.SetActive(true);
                //等1秒確保管理器以開啟
                yield return new WaitForSeconds(1.0f);
                //開啟時間賽
                StartCoroutine(ThrowGame_TimerGame.Instance.startGame());
                break;
        }
    }

	void endGame()
	{
		StartCoroutine(music_mgr.StopMusic());
		enableAllButton();

		if(mode==Mode.TIMER_MODE)
			message.keepShow("時間賽");
		else if(mode==Mode.NORMAL_MODE)
			message.keepShow("標準賽");

		gameState = StateType.UNSTART;
	}

    //手點擊按鈕請呼叫此函式
    public void pressButton(string btn_name)
    {
        switch(btn_name)
        {
            case "Start":
                gameState = StateType.START_GAME;
                break;
            case "TimerMode":
                message.keepShow("時間賽");
                mode = Mode.TIMER_MODE;
                btn_NormalMode.setEnable(true);
                break;
            case "NormalMode":
                message.keepShow("標準賽");
                mode = Mode.NORMAL_MODE;
                btn_TimerMode.setEnable(true);
                break;
        }
    }

    void disableAllButton()
    {
        btn_Start.gameObject.SetActive(false);
        btn_TimerMode.gameObject.SetActive(false);
        btn_NormalMode.gameObject.SetActive(false);

        //通知手 button 隱藏了
        ThrowGame_Button[] btns = new ThrowGame_Button[3];
        btns[0] = btn_Start;
        btns[1] = btn_TimerMode;
        btns[2] = btn_NormalMode;
		if(GameObject.FindGameObjectWithTag("Hand"))
        	GameObject.FindGameObjectWithTag("Hand").GetComponent<Hand>().buttonIsDisable(btns);
    }

    void enableAllButton()
    {
        btn_Start.gameObject.SetActive(true);
        btn_TimerMode.gameObject.SetActive(true);
        btn_NormalMode.gameObject.SetActive(true);
		btn_Start.setEnable(true);
    }
}
