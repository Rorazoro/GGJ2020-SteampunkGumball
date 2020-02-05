using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerator : MonoBehaviour
{
    public float force = 2;

    private void OnCollisionStay(Collision other) {
        Gumball ball = other.gameObject.GetComponent<Gumball>();
        if (ball != null)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.right * force, ForceMode.Acceleration);
        }
    }
}
