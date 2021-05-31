using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu sınıf bütün durum değişikliklerinin kontrol edildiği singleton sınıfıdır.

public class StateMachine : MonoBehaviour
{
    public IState currentState { get; set; }            // Mevcut durumun tutulduğu IState değişkeni
    public IState previousState;                        // Bir önceki durumun tutulduğu IState değişkeni

    private bool _inTransition = false;                 // Durum geçişini kontrol eden değişken

    public void ChangeState(IState newState) {          // Durum değiştirme metodu
        if (currentState == newState || _inTransition)
        {
            return;
        }
        ChangeStateRoutine(newState);
    }

    //public void RevertState() {                         // Bir önceki duruma geri dönme Metodu
    //    if (previousState != null) {
    //        currentState = previousState;
    //    }
    //}

    public void ChangeStateRoutine(IState newState) {   // Durum değiştirme Metodu
        _inTransition = true;                           // Durum değişme aşamasında
        if (currentState != null)
        {
            currentState.Exit();                        // IState Exit Metodunun çalıştığı yer
        }
        if (previousState != null) 
        {
            previousState = currentState;               // Önceki duruma şimdiki durum atandı
        }
        Debug.Log("New State");
        currentState = newState;                        // Yeni durum şimdiki duruma geldi

        if (currentState != null) 
        {
            currentState.Enter();                       // Yeni duruma giriş yapıldı
        }
        _inTransition = false;                          // Durum değişimi süreci sona erdi
    }
    // Update is called once per frame
    public void Update()                                // Unitynin bize sunduğu gameloop patterninin çalıştığı saniyede belli bir sayıda (FPS) çalışan döngü
    {
        if (currentState != null && !_inTransition)     // Durum null değilse ve geçiş yoksa IState'in Tick metodununu çağırır
        {
            currentState.Tick();
        }   
    }

    //public void FixedUpdate()                     // FixedUpdate Fizik objelerinin çalışacağı metod
    //{
    //    if (currentState != null && !_inTransition) 
    //    {
    //        currentState.FixTick();
    //    }    
    //}

}
