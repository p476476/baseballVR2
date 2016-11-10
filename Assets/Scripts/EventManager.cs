using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
    private static EventManager eventManager;
    private Dictionary<string, UnityEvent> eventDictionary;

    public static EventManager instance {
        get
        {
            //如果沒有eventManager
            if(!eventManager)
            {
                //找找看
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                //沒有找到
                if(!eventManager)
                {
                    Debug.LogError("There need to be one active EventManager script on a GameObject in your game.");
                }else
                {
                    //event Manager INITALIZZZZE this
                    eventManager.Init();
                }
            }
            return eventManager;
        }

    }

    void Init()
    {
        //沒有字典
        if(eventDictionary == null)
        {
            //新增一個字典
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

   


    public static void StartListening(string eventName,UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if(instance.eventDictionary.TryGetValue(eventName,out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName,UnityAction listener)
    {
        if (eventManager == null) return;

        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if(instance.eventDictionary.TryGetValue(eventName,out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
