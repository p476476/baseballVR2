using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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
    }

	void Update () {

        if (gamestart)
        {
            if (Input.GetKeyDown(KeyCode.Z) && IsbackPos)
            {
                getReady.Invoke();
                IsbackPos = false;
            }


            if (controller.TriggerButtonDown && IsbackPos)
            {
                getReady.Invoke();
                IsbackPos = false;
            }
        }
		
		


	}

	void GameStart (){


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
