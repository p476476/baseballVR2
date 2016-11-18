using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyKeyboard : MonoBehaviour {

	public static MyKeyboard Instance;

	public Text txt_input;

	public bool isCaps = true;

    int name_max_length = 16;
    public bool isEnterPressed = false;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
		
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
        EventManager.TriggerEvent("changeCase");
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
