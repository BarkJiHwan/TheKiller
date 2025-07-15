using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneContllor : MonoBehaviour
{
    public void GameStartBtn()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
