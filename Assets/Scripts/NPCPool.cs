using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPool : MonoBehaviour
{
    [SerializeField] private GameObject[] npcPrefab;
    private Queue<GameObject> _npcQueue = new Queue<GameObject>();

    public GameObject GetObject(Transform spawnPoint, PatrolGroup patrolGroup, Animator animator, float alertDistance)
    {
        GameObject obj;
        if (_npcQueue.Count > 0)
        {
            obj = _npcQueue.Dequeue();
        }
        else
        {
            int randomNpcIndex = Random.Range(0, npcPrefab.Length);
            obj = Instantiate(npcPrefab[randomNpcIndex]);
        }

        // 상태 초기화
        obj.transform.position = spawnPoint.position;

        animator = obj.GetComponent<Animator>() ?? obj.AddComponent<Animator>();

        NPCActions npc = obj.GetComponent<NPCActions>();
        Transform coverPoint = (patrolGroup.patrols != null && patrolGroup.patrols.Length > 0) ? patrolGroup.patrols[0].transform : null;
        npc.Initialize(patrolGroup.patrols, coverPoint, alertDistance);
        npc.PatrolSpeed = Random.Range(2f, 5f);
        npc.alertDistance = Random.Range(1f, 2f);

        NPCController npcController = npc.GetComponent<NPCController>();
        if (npcController != null)
        {
            npcController.patrolGroup = patrolGroup;
            npcController.coverPoint = coverPoint;
        }

        obj.SetActive(true);
        return obj;
    }

    // NPC 오브젝트 반납
    public void ReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
        _npcQueue.Enqueue(obj);
    }
}