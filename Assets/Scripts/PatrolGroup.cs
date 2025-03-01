using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGroup : MonoBehaviour
{
    public PatrolPoint[] patrols;
    void Awake()
    {
        // 자식 오브젝트에서 PatrolPoint 컴포넌트를 가져와서 배열로 초기화
        patrols = GetComponentsInChildren<PatrolPoint>();
    }

    public PatrolPoint[] GetPatrolPoints()
    {
        return patrols;
    }
}
