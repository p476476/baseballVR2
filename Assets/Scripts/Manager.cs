using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Manager : MonoBehaviour {

	public ControllerBase controller;

	private bool isInProgress = false;

	[SerializeField]
	private UnityEvent getReady;


	void Start () {
		
	}

	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Z)) {
			getReady.Invoke ();
		}


		if (controller.TriggerButtonDown) {
			getReady.Invoke ();
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
