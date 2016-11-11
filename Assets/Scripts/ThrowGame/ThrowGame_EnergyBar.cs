using UnityEngine;
using System.Collections;

public class ThrowGame_EnergyBar : MonoBehaviour
{

    public float energy_value = 0f;
    float max_energy_value = 100;
    float init_pos = -2f;
    float max_pos = 2;
    float current_pos;


    public void addEnergy(float value)
    {
        if (energy_value < max_energy_value)
        {
            energy_value += value;
            if (value > max_energy_value)
                value = max_energy_value;
        }
    }




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        current_pos = Mathf.Lerp(init_pos, max_pos, energy_value / max_energy_value);
        transform.Translate(new Vector3(0, current_pos - transform.position.y, 0));

        if (energy_value > max_energy_value)
            EventManager.TriggerEvent("energy");
    }

    public void init()
    {
        energy_value = 0f;
    }    
        

    //===================Unity Event========================//
    void OnEnable()
    {
        EventManager.StartListening("consume_energy", consume_energy);
    }

    void OnDisable()
    {
        EventManager.StopListening("consume_energy", consume_energy);
    }

    void consume_energy()
    {
        energy_value = 0;
    }
}
