using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class score_tuple
{
    public string name;
    public string date;
    public int score;
}

[System.Serializable]
public class ScoreDatabase
{
    [XmlArray("TimerGameScore")]
    public List<score_tuple> timerScoreList = new List<score_tuple>();

    [XmlArray("NormalGameScore")]
    public List<score_tuple> normalScoreList = new List<score_tuple>();
}


public class ThrowGame_ScoreRecorder : MonoBehaviour { 

    public static ThrowGame_ScoreRecorder Instance;

    public ScoreDatabase scoreDB;
    public TextMesh txt_score;
    public TextMesh txt_name;
    bool isChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        loadScore();
        sort();
        isChanged = true;
    }

    void Update()
    {
        if (isChanged)
        {
            updateScore();
            isChanged = false;
        }
    }

    //儲存資料到Xml
    void saveScore()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/Score_data.xml", FileMode.Create);
        serializer.Serialize(stream, scoreDB);
        stream.Close();
    }
    
    //讀取Xml資料
    void loadScore()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/Score_data.xml", FileMode.Open);
        scoreDB = serializer.Deserialize(stream) as ScoreDatabase;
        stream.Close();
    }

    //取得DataBase
    public ScoreDatabase getScores()
    {
        loadScore();
        return scoreDB;
    }

    //是否進排行(前五名)
    public bool isHighScore(int input_score)
    {
        List<score_tuple> scores = new List<score_tuple>();
        switch (ThrowGameManager.Instance.mode)
        {
            case ThrowGameManager.Mode.NORMAL_MODE:
                scores = scoreDB.normalScoreList;
                break;
            case ThrowGameManager.Mode.TIMER_MODE:
                scores = scoreDB.timerScoreList;
                break;
        }

        if(scores.Count<5)
        {
                return true;
        }
        else
        {
            if (input_score > scores[4].score)
                return true;
        }

        return false;
    }

    //新增一筆成績紀錄
    public void addScore(score_tuple one_score,ThrowGameManager.Mode mode)
    {
        Debug.Log("Add Score");
        
        switch(mode)
        {
            case ThrowGameManager.Mode.NORMAL_MODE:
                    scoreDB.normalScoreList.Add(one_score);
                break;
            case ThrowGameManager.Mode.TIMER_MODE:
                    scoreDB.timerScoreList.Add(one_score);
                break;
                
        }
        
        //排序
        sort();
        //儲存
        saveScore();
        //更新排行榜顯示
        isChanged = true;
    }

    //更新排行榜顯示資料
    void updateScore()
    {
        //update score text;
        List<score_tuple> scores = new List<score_tuple>();
        string str_name = string.Format("{0,0}\n", "Name");
        string str_score = string.Format("{0,0}\n", "Score");
        switch (ThrowGameManager.Instance.mode)
        {
            case ThrowGameManager.Mode.NORMAL_MODE:
                scores = scoreDB.normalScoreList;
                break;
            case ThrowGameManager.Mode.TIMER_MODE:
                scores = scoreDB.timerScoreList;
                break;

        }
        for (int i = 0; i < 5 && i < scores.Count; i++)
        {
            str_name += string.Format("{0,0}\n", scores[i].name);
            str_score += string.Format("{0,0}\n", scores[i].score);
        }
        txt_score.text = str_score;
        txt_name.text = str_name;
    }

    //排序
    void sort()
    {
        if (scoreDB.normalScoreList.Count > 0)
        {
            scoreDB.normalScoreList.Sort(delegate (score_tuple a, score_tuple b)
            {
                return (b.score).CompareTo(a.score);
            });
        }

        if (scoreDB.timerScoreList.Count > 0)
        {
            scoreDB.timerScoreList.Sort(delegate (score_tuple a, score_tuple b)
            {
                return (b.score).CompareTo(a.score);
            });
        }
    }
}
