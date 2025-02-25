using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyUiChanger : MonoBehaviour
{
    public Toggle toggle;

    public Button button;

    public void OnEnable()
    {
        toggle = this.GetComponent<Toggle>();        
    }

    public void onChange()
    {
        button.interactable = true;
    }

    public void JoinOn()
    {
    }
}
