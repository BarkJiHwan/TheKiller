using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float spinSpeed = 1000f;    
    public GameObject bulletMarkPrefab;

    private bool hasHit = false;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        if (!hasHit)
        { 
            ShootRaycast();
        }
        hasHit = false;
    }

    private void ShootRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f))
        {            
            OnBulletHit(hit);
        }
    }
    private void OnBulletHit(RaycastHit hit)
    {
        if (!hasHit)
        {
            NPCController npc = hit.collider.GetComponentInParent<NPCController>();

            if (npc != null)
            {
                npc.RayHit(hit.point, hit.normal, hit.collider.tag);
                hasHit = true;
            }
        }
    }
}