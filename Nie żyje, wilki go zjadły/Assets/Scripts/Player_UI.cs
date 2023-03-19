using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI dialogBox;
    [SerializeField] Image dialogBoxBackground;

    public void UpdateUIdialogBox(string text) {
        dialogBoxBackground.enabled = true;
        dialogBox.text = text;
    }

    public void CloseDialogBox() {
        dialogBox.text = "";
        dialogBoxBackground.enabled = false;
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
