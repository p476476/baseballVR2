using UnityEngine;
using System.Collections;

public class ThrowGame_MusicManager : MonoBehaviour {
	public static ThrowGame_MusicManager Instance;

    //music控制
    int current_num; //目前第?首 start at 0
    public int max_num;
    float volume;

    //Bool
    bool isPlaying ;
    bool keepVolumeDown = false;


	//audio sources
	public AudioSource  audio_countdown;

	//background music
	public new AudioSource audio;
	AudioClip[] music; //all music

	void Awake()
	{
		Instance = this;
	}

    // Use this for initialization
    void Start()
    {

        //set music
        music = Resources.LoadAll<AudioClip>("Music");
        audio = this.GetComponent<AudioSource>();
        current_num = 0;
        max_num = music.Length - 1;
        isPlaying = false;
        setSong(current_num);
        audio.Stop();
        volume = audio.volume;

    }

    // Update is called once per frame

    void Update()
    {

        //if current music is end play next song
        if (audio.clip.length - audio.time < 0.5f)
        {
            playNextSong();
        }

        //音樂暫停時慢慢變小聲
        if (keepVolumeDown)
        {
            audio.volume -= 0.03f;
            
        }

    }

    public void playMusic()
    {
        print("play music");
        if (!audio.isPlaying)
        {
            audio.volume = volume;
            audio.Play();
            isPlaying = true;
        }
    }

    public IEnumerator StopMusic()
    {
        print("stop music");
        isPlaying = false;
        keepVolumeDown = true;
        yield return new WaitForSeconds(1.0f);
        print("stop music2");
        keepVolumeDown = false;
        audio.Stop();

    }

    void setSong(int song_num)
    {
        audio.clip = music[current_num];
        audio.time = 0;
    }

    public void playNextSong()
    {
        print("Now Play Next Song!");
        audio.Stop();
        current_num += 1;
        if (current_num > max_num) current_num = 0;
        setSong(current_num);
        audio.Play();
    }

    public void playPreviousSong()
    {
        audio.Stop();
        current_num -= 1;
        if (current_num < 0) current_num = max_num;
        setSong(current_num);
        audio.Play();
    }


    string getTimeString(float time)
    {
        int min, sec;
        min = (int)(time / 60);
        sec = (int)(time % 60);

        string str = min.ToString();
        str += ":";
        if (sec < 10f) str += "0";
        str += "" + sec;

        return str;
    }

	public void playCountDownSound(){
		audio_countdown.Play ();
	}
}




