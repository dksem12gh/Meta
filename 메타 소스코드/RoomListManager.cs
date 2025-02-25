using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviour
{
    [SerializeField] Button _button;

    private void Awake()
    {
        _button.interactable = false;
    }

    private void OnEnable()
    {
        _button.interactable = false;
    }
}
