using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float force = 20000;

    private void OnCollisionStay(Collision other) {
        Gumball ball = other.gameObject.GetComponent<Gumball>();
        if (ball != null)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * force, ForceMode.Impulse);
        }
    }
}
