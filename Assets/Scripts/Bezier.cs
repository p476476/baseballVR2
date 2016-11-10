using UnityEngine;
using System.Collections;
using System;

public class Bezier {
    Vector3[] points;
    float total_time;//曲線走完所花的時間
    float current_time;//過了多少時間(start at 0)
    //用於變速
    float speed =1f;
    float start_speed = 1f; //1 = 正常速度; 2 = 2倍速度...
    float final_speed=1f;

    public void setBezier(Vector3[] pts,float time)
    {
        setPoints(pts);
        setTotalTime(time);
        current_time = 0;
    }

    void setTotalTime(float time)
    {
        total_time = time;
    }

    public void setCurrentTime(float time)
    {
        current_time = time;
    }

    public void addCurrentTime(float time)
    {
        current_time += time* speed;

        //speed = start_speed & final_speed 做內插
        speed = start_speed *(total_time- current_time) / total_time+final_speed * current_time / total_time;
    }

    public float getCurrentTime()
    {
        return current_time;
    }

    void setPoints(Vector3[] pts)
    {
        points = pts;
    }

    public void setSpeed(float start_s,float final_s) //設定變速
    {
        start_speed = start_s;
        final_speed = final_s;
    }

    public Vector3 GetCurrentSpeed()//得到當前速度
    {
        if (points.Length == 2)
        {
            return
                (points[1] - points[0]) / total_time * speed;
        }
        if (points.Length == 3)
        {
            return
                (2f * (total_time - current_time)  / total_time * (points[1] - points[0]) +
                2f * current_time / total_time  * (points[2] - points[1]))/total_time * speed;
        }else if(points.Length == 4)
        {
            float t = current_time/ total_time;
            return
                (3f * (1- t) * (1 - t) * (points[1] - points[0]) +
                6f * (1 - t) * t * (points[2] - points[1]) +
                3f * t * t * (points[3] - points[2]))/ total_time* speed;
        }
        return new Vector3(0, 0, 0);
    }
    public Vector3 GetCurrentPosition()//得到當前位置
    {
        if (points.Length == 2)
        {
            float t = current_time / total_time;
            return
                (points[1]*t + points[0]*(1- t));
        }
        if (points.Length == 3)
        {
            float t = current_time / total_time;
            return
                (1f*points[2] * t * t+
                 2f*points[1] * t * (1- t)+
                 1f*points[0] * (1 - t) * (1 - t));
        }
        else if (points.Length == 4)
        {
            float t = current_time / total_time;
            return
                (1f * points[3] * t * t * t +
                 3f * points[2] * t * t * (1 - t) +
                 3f * points[1] * t * (1 - t) * (1 - t) +
                 1f * points[0] * (1 - t) * (1 - t) * (1 - t));
        }
        return new Vector3(0, 0, 0);
    }

	public bool isArrived(){ // 抵達目標?
		if (current_time < total_time)
			return false;
		else
			return true;
	}


}
