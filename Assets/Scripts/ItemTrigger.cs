﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using DungeonOfVinheim;

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
    Up
}

public class AnimationEvent : UnityEvent{}

// Collider may not be necessary can delete later on
public class TriggerEvent : UnityEvent<Collider, GameObject>{
    public TriggerEventType type;
    public Direction direction;
}

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

    public void DoorFullyOpen(){
        entranceFullyOpenEvent.Invoke();
    }
    
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject == GameManager.localPlayerInstance)
        {
            UIManager.setActionTextContentEvent.Invoke(message, prefix);
            UIManager.setActionTextActiveEvent.Invoke(true);
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject == GameManager.localPlayerInstance)
        {
            UIManager.setActionTextActiveEvent.Invoke(false);
        }
    }
    
    void OnTriggerStay(Collider collider){
        if(collider.gameObject == GameManager.localPlayerInstance){
            if(type == TriggerEventType.Entrance) triggerEventStay.direction = direction;
            triggerEventStay.type = type;
            triggerEventStay.Invoke(collider, this.gameObject);
        }
    }
}
