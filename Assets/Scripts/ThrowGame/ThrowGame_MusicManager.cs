using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrowGame_MusicManager : MonoBehaviour {
	public static ThrowGame_MusicManager Instance;

    //音量控制
    float volume = 0.5f;

    //Bool
    bool isPlaying = false;
    bool keepVolumeDown = false;//用於換音樂時 先將前一首歌 音量逐漸縮小

	//音效
	public AudioSource  audio_countdown;
    public AudioSource audio_gathering;
    public AudioSource audio_button;

    //背景音樂
    public new AudioSource audio;
	AudioClip[] music; //all music
    Dictionary<string, AudioClip> music_dictionary = new Dictionary<string, AudioClip>();

	void Awake()
	{
		Instance = this;

        //set music
        music = Resources.LoadAll<AudioClip>("Music");
        audio = this.GetComponent<AudioSource>();

        foreach (AudioClip a in music)
        {
            print(a.name);
            music_dictionary.Add(a.name, a);
        }
    }


    void Start()
    {


    }


    void Update()
    {
        //音樂播完時 下一首
        if (audio.isPlaying&&audio.clip.length - audio.time < 0.5f && ThrowGameManager.Instance.gameState == ThrowGameManager.StateType.UNSTART)
        {
            playRandomMusic();
        }


        //音樂暫停時慢慢變小聲
        if (keepVolumeDown)
        {
            audio.volume -= 0.03f;

        }

    }

    public void playRandomMusic()
    {
        print("playRandomMusic");
        int index = Random.Range(0, music.Length);

        //不要播到遊戲時的音樂
        while(music[index].name == "Invisible"|| (music[index].name == "Lagoa_v2"))
        {
            index = Random.Range(0, music.Length);
        }

        StartCoroutine(playMusic(music[index].name));
    }

    public IEnumerator playMusic(string songName)
    {
        print("play music");

        if (isPlaying)
        {
            StartCoroutine(StopMusic());
            yield return new WaitForSeconds(1.0f);
        }

        audio.clip = music_dictionary[songName];
        audio.volume = volume;
        audio.Play();
        audio.loop = true;
        isPlaying = true;
    }

    public IEnumerator StopMusic()
    {
        print("stoping music");
        isPlaying = false;
        keepVolumeDown = true;
        yield return new WaitForSeconds(1.0f);
        print("music stopped");
        keepVolumeDown = false;
        audio.Stop();

    }


	public void playCountDownSound(){
		audio_countdown.Play ();
	}

    public void playCheerSound()
    {

    }

    public void playGatheringSound()
    {
        audio_gathering.Play();
    }

    public void stopGatheringSound()
    {
        audio_gathering.Stop();
    }



}




