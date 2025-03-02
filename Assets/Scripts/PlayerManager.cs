using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public GameObject player; // 플레이어 오브젝트
    public Transform[] stageSpawnPoints; // 스테이지별 스폰 포인트

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerManager 인스턴스가 생성되었습니다");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("PlayerManager 인스턴스가 이미 존재하여 중복된 오브젝트를 파괴합니다");
        }
    }

    private void Start()
    {
        Debug.Log("PlayerManager Start 메서드가 호출되었습니다");

        // 게임 시작 시 플레이어의 위치를 첫 번째 스폰 포인트로 이동
        MovePlayerToSpawnPoint(0);
    }

    // 플레이어를 지정된 스폰 포인트로 이동
    public void MovePlayerToSpawnPoint(int spawnIndex)
    {
        if (player == null)
        {
            Debug.LogError("Player 오브젝트가 할당되지 않았거나 파괴되었습니다");
            return;
        }

        if (spawnIndex >= 0 && spawnIndex < stageSpawnPoints.Length)
        {
            player.transform.position = stageSpawnPoints[spawnIndex].position;
            player.transform.rotation = stageSpawnPoints[spawnIndex].rotation;
            Debug.Log("플레이어가 스폰 포인트로 이동되었습니다: " + stageSpawnPoints[spawnIndex].position);
        }        
    }
}