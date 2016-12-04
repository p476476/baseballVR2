using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audio : MonoBehaviour {

    AudioSource audio, audioFi;
    List<AudioSource> sound = new List<AudioSource>();
    AudioClip[] music;

    Dictionary<string, AudioClip> dic_music = new Dictionary<string, AudioClip>();
    // Use this for initialization
    void Start() {
       // audio = GetComponent<AudioSource>();
        GetComponents<AudioSource>(sound);
        
        music = Resources.LoadAll<AudioClip>("StrikeMusic");
        foreach (AudioClip a in music)
        {
            dic_music.Add(a.name, a);
        }
    }

    // Update is called once per frame
    public void Strike()
    {
        sound[1].clip = dic_music["strike"];
        sound[1].volume = 1.0f;
        sound[1].Play();
    }
    public void Cheers()
    {
        sound[1].clip = dic_music["cheers"];
        sound[1].volume = 0.5f;
        sound[1].Play();
    }
    public void foul_ball()
    {
        sound[1].clip = dic_music["foul_ball"];
        sound[1].volume = 1.0f;
        sound[1].Play();
    }
    public void firework()
    {
        sound[0].clip = dic_music["explosion"];
        sound[0].Play();
    }
    
}
