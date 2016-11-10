using UnityEngine;
using System.Collections;

public class ThrowGame_Ball : MonoBehaviour {

    //是否沒打到數字牌
    bool isMissBall = true;
	bool isDroped = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(transform.up* 0.03f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(transform.up * -0.03f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(transform.right * -0.03f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(transform.right * 0.03f);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            this.transform.Translate(transform.forward * 0.03f);
        }
        if (Input.GetKey(KeyCode.X))
        {
            this.transform.Translate(transform.forward * -0.03f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            if (isMissBall &&
                ThrowGameManager.Instance.mode == ThrowGameManager.Mode.NORMAL_MODE &&
                ThrowGameManager.Instance.gameState == ThrowGameManager.StateType.PLAYING)
            {
                ThrowGame_NormalGame.Instance.missHit();
            }

			if (!isDroped) {
				ThrowGame_NormalGame.Instance.ballDropOnGround ();
				isDroped = true;
			}

            Destroy(this.gameObject, 3f);
        }

        if (collision.gameObject.tag == "Number")
        {
            isMissBall = false;
        }
    }
}
