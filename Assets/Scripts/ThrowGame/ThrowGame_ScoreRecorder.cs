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
    public List<score_tuple> scoreList = new List<score_tuple>();
}



public class ThrowGame_ScoreRecorder : MonoBehaviour { 

    public static ThrowGame_ScoreRecorder Instance;

    public ScoreDatabase scoreDB;
    public TextMesh txt_score;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        loadScore();
    }

    void Update()
    {
        //update score text;
        string str = string.Format("{0,-10} {1,10}\n", "Name", "Score");
        for (int i = 0; i < 10 && i < scoreDB.scoreList.Count; i++)
        {
            str += string.Format("{0,-10} {1,10}\n", scoreDB.scoreList[i].name, scoreDB.scoreList[i].score);
        }
        txt_score.text = str;
    }

    void saveScore()
    {//初始化XML
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamFiles/XML/Score_data.xml",FileMode.Create);
        serializer.Serialize(stream, scoreDB);
        stream.Close();


    }
    
    void loadScore()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamFiles/XML/Score_data.xml", FileMode.Open);
        scoreDB = serializer.Deserialize(stream) as ScoreDatabase;
        stream.Close();
    }

    public ScoreDatabase getScores()
    {
        loadScore();
        return scoreDB;
    }

    public void addScore(score_tuple one_score)
    {
        Debug.Log("Add Score");
    
        scoreDB.scoreList.Add(one_score);
        sort();
        saveScore();
    }

    void sort()
    {
        if (scoreDB.scoreList.Count > 0)
        {
            scoreDB.scoreList.Sort(delegate (score_tuple a, score_tuple b)
            {
                return (a.score).CompareTo(a.score);
            });
        }
    }
}
