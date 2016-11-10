using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TestEvent : MonoBehaviour {

    private UnityAction someListener;

    void Awake()
    {
        someListener = new UnityAction(someListener);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}
