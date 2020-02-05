using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMotor : MonoBehaviour
{
    public bool rotateClockwise = true;

    // Update is called once per frame
    void Update()
    {
        if (rotateClockwise) {
            transform.Rotate(0, 0, -30 * Time.deltaTime);
        }
        else {
            transform.Rotate(0, 0, 30 * Time.deltaTime);
        }
        
    }
}
