using UnityEngine;
using System.Collections;

public class effect : MonoBehaviour {

    public ParticleSystem firework;
    GameObject admin;
    int lasParNum = -1/**/, currentParNum;
	// Use this for initialization
	void Start () {
        admin = GameObject.FindGameObjectWithTag("Admin");
	}
	void Update()
    {
        currentParNum = firework.particleCount;
        if (currentParNum < lasParNum)
        {
            admin.GetComponent<Audio>().firework();         //煙火爆破聲
        }
        lasParNum = currentParNum;
    }
	// Update is called once per frame
	public void ShootFireWork()
    {
        firework.Play();
        
    }
}
