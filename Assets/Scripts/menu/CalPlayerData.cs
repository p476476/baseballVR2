using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CalPlayerData : MonoBehaviour
{

    public GameObject ResultBoard;
    public GameObject displayLife;

    public int score;
    private Rigidbody BoardRigidbody;       //ResultBoard的剛體
    private TextMesh rightColTM;                 //rightCol的TextMesh


    private int[] rightCol;                 //ResultBoard中最右邊的欄位
    private int[] getScore;
    public int life = 10; //玩家生命
    bool lastHit;                           //上一球是否打中

    // Use this for initialization
    void Start()
    {
        score = 0;
        lastHit = false;
        BoardRigidbody = ResultBoard.GetComponent<Rigidbody>();

        rightColTM = ResultBoard.transform.FindChild("rightCol").gameObject.GetComponent<TextMesh>();

        rightCol = new int[9];
        getScore = new int[8] { 200, 300, 400, 500, 600, 200, 3000, 0 };
    }
    /*void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            score = 0;
            
        }
    }*/
    //連續hit生命++，miss則--
    public void lifeUpdate(bool hit)
    {
        if (hit && lastHit && life < 10) life++;
        else if (hit && !lastHit) lastHit = true;
        else if (!hit)
        {
            lastHit = false;
            life--;
            if (life == 0) //當生命歸零，會掉一個記分板下來
            {
                //Board掉落
                ResultBoard.SetActive(true);
                Vector3 speedVec = new Vector3(0.0f, -200.0f, 0.0f);
                BoardRigidbody.velocity = speedVec;

                //righCol 文字輸入
                for (int i = 0; i < 8; i++)
                {
                    rightColTM.text += rightCol[i].ToString() + "\n";
                    rightCol[8] += getScore[i] * rightCol[i];
                }
                rightColTM.text += "\n" + rightCol[8].ToString();
            }
        }
        displayLife.GetComponent<TextMesh>().text = life.ToString();
    }
    public void Strike(int index)
    {
        rightCol[index]++;
    }
    public void distanceScore(Transform ball)    //尚未考慮界內界外問題
    {
        float dis = Vector3.Distance(ball.position, GameObject.Find("StrikeZone").transform.position);
        score += (int)dis * 10;

    }


}
