using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 100f;
    public GameObject SparkEffectPrefab;

    void Start()
    {        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "NPC" || collision.transform.tag == "NPCHead")
        {
            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 hitPosition = contactPoint.point;
            Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);
            var Blood = Instantiate(SparkEffectPrefab, hitPosition, rotation);
            Blood.transform.parent = transform;           
        }
    }
}
