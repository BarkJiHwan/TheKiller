using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("NPC 풀 설정")]
    [SerializeField] private int poolSize = 1;

    [Header("스폰 설정")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform coverPoint; // 커버 포인트

    [Header("NPC 행동 설정")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float alertDistance;        

    [Header("애니메이션 설정")]
    [SerializeField] private Animator _animator;

    [SerializeField] private UIManager _uiManager;
    [SerializeField] private List<NPCPool> _npcPool;

    public List<NPCActions> npcs = new List<NPCActions>();
    public List<NPCActions> defeatedNpcs = new List<NPCActions>();

    public void InitializeNPCs(PatrolGroup patrolGroup)
    {
        for (int i = 0; i < poolSize; i++)
        {
            Transform spawnPoint = spawnPoints[StageMgr.CurrentStage];
            GameObject npcObj = _npcPool[StageMgr.CurrentStage].GetObject(
                spawnPoint, patrolGroup, _animator, alertDistance
            );
            NPCActions npc = npcObj.GetComponent<NPCActions>();
            npcs.Add(npc);
        }
    }

    // NPC가 죽었을 때 Pool에 반환
    public void RemoveNPC(NPCActions npc)
    {
        if (npcs.Contains(npc))
        {
            defeatedNpcs.Add(npc);
            npcs.Remove(npc);
            _npcPool[StageMgr.CurrentStage].ReleaseObject(npc.gameObject);
        }
    }
    
    public bool CheckAllNPCsDead()
    {
        foreach (var npc in npcs)
        {
            if (!npc.isDead)
            {
                return false;
            }
        }
        npcs.Clear();
        return true;
    }
}
