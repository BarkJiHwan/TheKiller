using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatrolGroup : MonoBehaviour
{
    public PatrolPoint[] patrols;
    


    void Awake()
    {
        // 자식 오브젝트에서 PatrolPoint 컴포넌트를 가져와서 배열로 초기화
        patrols = GetComponentsInChildren<PatrolPoint>();
        
    }

    private void Start()
    {
    }
    public PatrolPoint[] GetPatrolPoints()
    {
        return patrols;
    }
}
//    public PatrolPoint[] patrols;
//    public GameObject particlePrefab; // 파티클 프리팹

//    private GameObject particleInstance;

//    void Awake()
//    {
//        // 자식 오브젝트에서 PatrolPoint 컴포넌트를 가져와서 배열로 초기화
//        patrols = GetComponentsInChildren<PatrolPoint>();
//    }
//    private void Start()
//    {
//        if (particlePrefab != null)
//        {
//            particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform);
//        }
//    }

//    public PatrolPoint[] GetPatrolPoints()
//    {
//        return patrols;
//    }
//}
