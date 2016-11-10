using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MyKeyboardButton : MonoBehaviour {

    Text txt;

    void OnStrat()
    {
        txt = GetComponentInChildren<Text>();
    }

    void OnEnable()
    {
        EventManager.StartListening("changeCase", changeCase);
    }

    void OnDisable()
    {
        EventManager.StopListening("changeCase", changeCase);
    }

    //變換大小寫
    void changeCase()
    {
        txt = GetComponentInChildren<Text>();


        if (txt.text.ToUpper() == txt.text)
            txt.text = txt.text.ToLower();
        else
            txt.text = txt.text.ToUpper();
    }
}
