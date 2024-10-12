using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Basics")]
    
    public float Coin;
    public float tuki;
    public float iagra;
    public float clona;

    public void AddCoin()
    {
        Coin += 1;
    }

    public void AddTuki()
    {
        tuki += 1;
    }
    public void AddIagra()
    {
        iagra += 1;
    }
    public void AddClona()
    {
        clona += 1;
    }
}
