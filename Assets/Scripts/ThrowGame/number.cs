using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class number : MonoBehaviour {

    //顯示的數字
    public int myNum;

    //在九宮格哪格位置
    public int myPos;

    //動畫
    Animator anim;
    
    //數字textMesh
    TextMesh myText;

    //是否被球打到過
    bool ishitted;

    //球打到板子的音效
    public AudioClip hitSound;

//=====================System Function=============================//
	void Start () {
        anim = GetComponent<Animator>();
        myText = GetComponentInChildren<TextMesh>();
        myText.text = "" + myNum;
        ishitted = false;

        //anim.Play("num_create");
        StartCoroutine(AnimColplete());
    }

    void Update()
    {

    }

//=====================My Function===================//


    //播放動畫結束時
    IEnumerator AnimColplete()
    {
        if (anim.enabled == true)
        {
            //當動畫播完
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length-0.1f);

            //暫停動畫
            anim.Stop() ;

            //物體轉到(0,90,0) 確保動畫不要轉超過
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            //使用物理效果
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && !ishitted)//被球打中
        {
            
            ishitted = true;

            //發出音效
            this.GetComponent<AudioSource>().PlayOneShot(hitSound,0.7f);

			//
			this.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,0.1f));

            //呼叫GM 執行打中的函式
            if(ThrowGameManager.Instance.mode == ThrowGameManager.Mode.NORMAL_MODE)
                ThrowGame_NormalGame.Instance.hitNumber(myNum);
            else if (ThrowGameManager.Instance.mode == ThrowGameManager.Mode.TIMER_MODE)
                ThrowGame_TimerGame.Instance.hitNumber(this);
        }

        //被球打中後掉到地板
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(this, 1f);
        }
    }





}
