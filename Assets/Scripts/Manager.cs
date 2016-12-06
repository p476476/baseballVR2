using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    
    public static Manager instance; 
	public ControllerBase controller;

	private bool isInProgress = false;
    public bool IsbackPos;
    public bool gamestart;
	[SerializeField]
	private UnityEvent getReady;
    void Awake()
    {
        instance = this;
    }

	void Start () {
        
        IsbackPos = true;
        gamestart = false;
        StartCoroutine(loadScenes());
    }

    //載入其他場景(回menu時會用到)
    IEnumerator loadScenes()
    {
        yield return SceneManager.CreateScene("practice");

        yield return SceneManager.CreateScene("menu");
    }

    void Update () {

        if (gamestart)
        {
            if (controller.TriggerButtonDown && IsbackPos)
            {
                getReady.Invoke();

            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && IsbackPos)
        {
            getReady.Invoke();

        }
    }

    
	public void GameStart(){
        StartCoroutine(WaitTime(1));

	}

    IEnumerator WaitTime(float _Time)
    {
        yield return new WaitForSeconds(_Time);
        gamestart = true;
    }

	void GameStop (){


	}
	void GameResume (){


	}
	void GameEnd (){


	}


	void inningStart (){


	}
	void inningStop (){


	}
	void inningResume (){


	}
	void inningEnd (){


	}









}
