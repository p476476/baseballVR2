using UnityEngine;
using System.Collections;

public class ThrowGame_BackgroundBall : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        GetComponentInChildren<ParticleSystem>().Emit(5);
    }
}
