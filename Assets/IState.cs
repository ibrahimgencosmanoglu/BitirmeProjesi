using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    bool isAvailable { get; set; }
    void Enter();
    void Tick();
    void FixTick();
    void Exit();
}
