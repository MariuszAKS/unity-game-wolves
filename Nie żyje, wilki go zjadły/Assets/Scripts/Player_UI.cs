using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI dialogBox;

    public void UpdateUIdialogBox(string text) {
        dialogBox.text = text;
    }

    public void ClearDialogBox() {
        dialogBox.text = "";
    }

    public void UpdateUIinfo(bool armL, bool armR, bool legL, bool legR, bool torso)
    {
        info.text = "";
        info.text += "Left arm: " + (armL ? "X" : "-");
        info.text += "\nRight arm: " + (armR ? "X" : "-");
        info.text += "\nLeft leg: " + (legL ? "X" : "-");
        info.text += "\nRight leg: " + (legR ? "X" : "-");
        info.text += "\nTorso: " + (torso ? "X" : "-");
    }
}
