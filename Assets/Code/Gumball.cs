using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gumball : MonoBehaviour
{
    private Rigidbody rg;

    private void Start() {
        rg = GetComponent<Rigidbody>();
        rg.sleepThreshold = 0.0f;
    }
}
