using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleController : MonoBehaviour
{
    [Range(0.0f, 24f)] public float hour = 0f;
    public Transform sun;
    float sunX;
    public float dayDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hour += Time.deltaTime * ( 24 / (60 * dayDuration));
        if(hour >= 24)
            hour = 0;

        SunRotation();
    }

    private void SunRotation()
    {
        sunX = 15 * hour;

        sun.localEulerAngles = new Vector3(sunX, 90, 90);

        if (hour < 1 || hour > 18)
            sun.GetComponent<Light>().intensity = 0;
        else
            sun.GetComponent<Light>().intensity = 1;
    }
}
