using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IState currentState { get; set; }
    public IState previousState;

    private bool _inTransition = false;

    public void ChangeState(IState newState) {
        if (currentState == newState || _inTransition)
        {
            Debug.Log("Same State");
            return;
        }
        ChangeStateRoutine(newState);
    }
    public void RevertState() {
        if (previousState != null) {
            currentState = previousState;
        }
    }
    public void ChangeStateRoutine(IState newState) {
        _inTransition = true;
        if (currentState != null) 
        {
            currentState.Exit();
        }
        if (previousState != null) 
        {
            previousState = currentState;
        }
        Debug.Log("New State");
        currentState = newState;

        if (currentState != null) 
        {
            currentState.Enter();
        }
        _inTransition = false;
    }
    // Update is called once per frame
    public void Update()
    {
        if (currentState != null && !_inTransition) 
        {
            currentState.Tick();
        }   
    }

    //public void FixedUpdate()
    //{
    //    if (currentState != null && !_inTransition) 
    //    {
    //        currentState.FixTick();
    //    }    
    //}

}
