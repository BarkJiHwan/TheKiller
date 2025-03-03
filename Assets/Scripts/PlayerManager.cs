using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject player; //플레이어 오브젝트
    public GameObject[] stageSpawnPoints; //스테이지별 스폰 포인트    
    private GameObject currentSpawnPointInstance; //현재 스폰 포인트 인스턴스

    private void Awake()
    {
        //싱글톤 구현
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

    //플레이어를 지정된 스폰 포인트로 이동
    public void MovePlayerToSpawnPoint(int stage)
    {
        if (player == null)
        {
            return;
        }
        if (currentSpawnPointInstance != null)
        {
            Destroy(currentSpawnPointInstance);
        }
        
        if (stage >= 0 && stage < stageSpawnPoints.Length)
        {
            player.transform.position = stageSpawnPoints[stage].transform.position;
            player.transform.rotation = stageSpawnPoints[stage].transform.rotation;
        }
    }

    public void ClearCurrentSpawnPoint()
    {
        if (currentSpawnPointInstance != null)
        {
            Destroy(currentSpawnPointInstance);
            currentSpawnPointInstance = null;
        }
    }
}