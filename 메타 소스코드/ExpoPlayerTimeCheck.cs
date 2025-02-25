using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ExpoPlayerTimeCheck : MonoBehaviour
{
    [SerializeField] TMP_Text _timeText;
     
    void Update()
    {
        _timeText.text = DateTime.Now.ToString("yyyy-MM-dd HH : mm : ss tt");
    }
}
