using System.Collections;           // C# namespace i
using System.Collections.Generic;   // C# namespace i
using UnityEngine;                  // Unity namespace i


// Bu interface kullanarak kullanıcı istediği durumu projesine entegre edebilir.

public interface IState 
{
    bool isAvailable { get; set; }  // Durumun durum makinasından olup olmadığını tutan değer
    void Enter();                   // Duruma giriş yapıldığında çalışacak olan metod
    void Tick();                    // Update döngüsünün içinde çalışan metod
    void FixTick();                 // Fixed Update içerisinde çalışan metod fizik nesnelerinin bu metodda çalıştırılması tavsiye edilir
    void Exit();                    // Durumdan çıkarken çalışacak olan metod
}
