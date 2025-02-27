using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 100f;    

    void Start()
    {        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("NPCHead"))
        {
            Debug.Log("헤드때림");
        }
        else if (collision.collider.CompareTag("NPCBody"))
        {
            Debug.Log("몸통때림");
        }
    }
}
