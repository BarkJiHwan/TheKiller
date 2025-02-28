using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform panelTransform;

    public Vector3 areaMinBounds => new Vector3(
        panelTransform.position.x - (panelTransform.localScale.x * 0.5f),
        panelTransform.position.y - (panelTransform.localScale.y * 0.5f),
        panelTransform.position.z - (panelTransform.localScale.z * 0.5f)
    );

    public Vector3 areaMaxBounds => new Vector3(
        panelTransform.position.x + (panelTransform.localScale.x * 0.5f),
        panelTransform.position.y + (panelTransform.localScale.y * 0.5f),
        panelTransform.position.z + (panelTransform.localScale.z * 0.5f)
        );

    public static GameManager Instance { get; private set; }

    public PatrolGroup patrolGroup1;
    //public PatrolGroup[] patrolGroup2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PatrolGroup GetPatrolGroup(int stage)
    {
        switch (stage)
        {
            case 1:
                patrolGroup1.gameObject.SetActive(true);
                return patrolGroup1;
            case 2:
                //return patrolGroup2;
            default:
                return null;
        }
    }
}