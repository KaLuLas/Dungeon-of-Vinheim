﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// might contiains other triggers later on like items or monsters...?
public enum TriggerEventType{
    Entrance,
    NULL
}
// direction that player enters the next room
public enum Direction{
    Down,
    Left,
    Right,
    Up,
    Center
}

public class AnimationEvent : UnityEvent{}

public class ItemTrigger : MonoBehaviourPun
{
    // Trigger Event will now bring the information of the entrance
    public TriggerEventType type;
    public Direction direction;
    public string message;
    public bool prefix;
    // NOTE: only have one triggerEventEnter (static)
    
    public static TriggerEvent triggerEventStay = new TriggerEvent();
    // public static TriggerEvent triggerEventEnter = new TriggerEvent();
    // public static TriggerEvent triggerEventExit = new TriggerEvent();
    public static AnimationEvent entranceFullyOpenEvent = new AnimationEvent();
    private UnityAction itemAction = null;

    public void DoorFullyOpen(){
        entranceFullyOpenEvent.Invoke();
    }
    
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject == GameManager.localPlayerInstance)
        {
            // if(collider.gameObject.tag == "Player")
            // {
                UIManager.setActionTextContentEvent.Invoke(message, prefix);
                UIManager.setActionTextActiveEvent.Invoke(true);
                if(type == TriggerEventType.Entrance){
                    itemAction = delegate () { GameManager.Instance.HandleEntranceEvent(direction); };
                    GameManager.ActionTriggerEvent.AddListener(itemAction);
                }
            // }
            // else if(collider.gameObject.tag == "Host")
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject == GameManager.localPlayerInstance)
        {
            // if (collider.gameObject.tag == "Player")
            // {
                UIManager.setActionTextActiveEvent.Invoke(false);
                if (itemAction != null)
                {
                    GameManager.ActionTriggerEvent.RemoveListener(itemAction);
                    itemAction = null;
                }
            // }
            // else if(collider.gameObject.tag == "Host")
        }
    }
}
