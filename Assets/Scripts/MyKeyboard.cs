using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyKeyboard : MonoBehaviour {

	MyKeyboard Inctance;

	public Text txt_input;

	public bool isCaps = true;

    int name_max_length = 16;


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
		
	}

    //按Caps時 呼叫此function
    public void pressCaps()
	{
		isCaps = !isCaps;
        EventManager.TriggerEvent("changeCase");
    }
}
