using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    // Ajustamos las paredes y puertas para solo tener 3 direcciones:
    // 0 - Up (Adelante), 1 - Right, 2 - Left
    public GameObject[] walls; // 0 - Up 1 - Right 2 - Left
    public GameObject[] doors; // Solo puertas para esas 3 direcciones

    public void UpdateRoom(bool[] status)
    {
        // Solo trabajaremos con 3 direcciones: adelante, derecha, izquierda
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}

