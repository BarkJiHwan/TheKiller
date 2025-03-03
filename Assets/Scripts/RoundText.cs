using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoundText : MonoBehaviour
{
    public TextMeshPro textMesh;
    

    public void UpdateRoundText(int round)
    {
        textMesh.text = $"Stage {round}";
    }
}