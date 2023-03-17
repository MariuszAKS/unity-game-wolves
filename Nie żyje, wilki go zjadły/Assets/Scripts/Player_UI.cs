using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    public void UpdateUI(bool armL, bool armR, bool legL, bool legR, bool torso)
    {
        textMesh.text = "";
        textMesh.text += "Left arm: " + (armL ? "X" : "-");
        textMesh.text += "\nRight arm: " + (armR ? "X" : "-");
        textMesh.text += "\nLeft leg: " + (legL ? "X" : "-");
        textMesh.text += "\nRight leg: " + (legR ? "X" : "-");
        textMesh.text += "\nTorso: " + (torso ? "X" : "-");
    }
}
