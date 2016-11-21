using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyKeyboard : MonoBehaviour {

	public static MyKeyboard Instance;

    //名字字串
	public Text txt_input;

    //大小寫轉換
	public bool isCaps = true;
    //是否按了Enter(輸入完畢)
    public bool isEnterPressed = false;

    //名字長度限制
    int name_max_length = 9;

    //三排按鍵
    public GameObject[] rows = new GameObject[3];

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.P))
        {
            pressBack();
        }
	}

    //按鍵click時 呼叫此function
	public void inputChar(string ch){
        //轉大寫
        if (isCaps)
            ch = ch.ToUpper();
        //限制名子長度
        if(txt_input.text.Length<name_max_length)
            txt_input.text += ch;
	}

    //按Enter時 呼叫此function
    public void pressEnter()
	{
        isEnterPressed = true;
    }

    //按Caps時 呼叫此function
    public void pressCaps()
	{
        isCaps = !isCaps;
        foreach (GameObject row in rows)
        {
            Text[] texts = GetComponentsInChildren<Text>();
            foreach(Text t in texts)
            {
                if(isCaps)
                {
                    t.ToString().ToUpper();
                }else
                {
                    t.ToString().ToLower();
                }
            }

        }
		
        EventManager.TriggerEvent("changeCase");
    }

    //按Backspace時 呼叫此function
    public void pressBack()
    {
        Debug.Log("pressBack");
        if(txt_input.text.Length>0)
            txt_input.text = txt_input.text.Substring(0, txt_input.text.Length - 1);
    }

    /*
    //===============Unity Event===============//
    void OnEnable()
    {
        EventManager.StartListening("enableKeyboard", enableKeyboard);
        EventManager.StartListening("disableKeyboard", disableKeyboard);
    }

    void OnDisable()
    {
        EventManager.StopListening("enableKeyboard", enableKeyboard);
        EventManager.StopListening("disableKeyboard", disableKeyboard);
    }*/

    public void enableKeyboard()
    {
        print("enableKeyboard");
        txt_input.text = "";
        isEnterPressed = false;
        this.GetComponent<RectTransform>().position = new Vector3(0.54f, 0.94f, 1.22f);
    }

    public void disableKeyboard()
    {
        print("disableKeyboard");
        this.GetComponent<RectTransform>().position = new Vector3(-50.54f, 0.94f, 1.22f);
    }

    public bool finishInput()
    {
        return isEnterPressed;
    }
}
