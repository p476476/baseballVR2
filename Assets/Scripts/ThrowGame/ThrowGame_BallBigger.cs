using UnityEngine;
using System.Collections;

public class ThrowGame_BallBigger : MonoBehaviour {

    float init_size = 0.3f;
    float current_size;
    public float maxsize = 1f;
    float max_radis = 0.5f;

    Transform[] children;

    bool beBigger = true;

    void Start()
    {
        children = transform.GetComponentsInChildren<Transform>();
        current_size = init_size;
    }


    // Update is called once per frame
    void Update()
    {
        if (current_size < maxsize)
            current_size += Time.deltaTime * 0.2f;

        if (beBigger)
        { 
            foreach (var child in children)
            {
                if (child != this.transform)
                    child.localScale = Vector3.one * current_size;
            }

            this.GetComponent<SphereCollider>().radius = max_radis * current_size;
         }
	}

    void ballStopBigger()
    {
        beBigger = false;
    }

    void OnEnable()
    {
        EventManager.StartListening("stopBiggerBall", ballStopBigger);
    }

    void OnDisable()
    {
        EventManager.StopListening("stopBiggerBall", ballStopBigger);
    }


}
