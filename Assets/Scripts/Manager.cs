using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Manager : MonoBehaviour {

    public static Manager instance; 
	public ControllerBase controller;

	private bool isInProgress = false;
    public bool IsbackPos;
	[SerializeField]
	private UnityEvent getReady;


	void Start () {
        instance = this;
        IsbackPos = true;
    }

	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Z) && IsbackPos) {
			getReady.Invoke ();
            IsbackPos = false;
		}


		if (controller.TriggerButtonDown && IsbackPos) {
			getReady.Invoke ();
            IsbackPos = false;
        }
		if (controller.testbuttonDown) {
			getReady.Invoke ();
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
