using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageBoundary : MonoBehaviour
{
    public Transform spawnPoint; // 현재 스테이지의 스폰 포인트

    private UIManager _uiManager;
    
    private void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 범위를 벗어날 때
        {
            other.transform.position = spawnPoint.position; // 플레이어를 스폰 포인트로 이동
            other.transform.rotation = spawnPoint.rotation;

            if (_uiManager != null)
            {
                _uiManager.ShowWarningMessage("You cannot leave this area!"); // 경고 메시지 표시
            }
        }
    }
}