﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class tests : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }
    }
}
