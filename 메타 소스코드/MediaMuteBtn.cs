using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaMuteBtn : MonoBehaviour
{
    [SerializeField] Sprite[] _sprite;
    [SerializeField] Slider _slider;
    [SerializeField] Image _img;

    bool _click = false;


    public void MiceClick()
    {
        if(!_click)
        {
            _click = true;
            _slider.value = 0.0f;
            _img.sprite = _sprite[1];
            _img.color = Color.red;
        }
        else
        {
            _click = false;
            _slider.value = 1.0f;
            _img.sprite = _sprite[0];
            _img.color = Color.white;
        }
    }

    public void VolumCheck()
    {
        if (_slider.value != 0.0f)
        {
            _img.sprite = _sprite[0];
            _img.color = Color.white;
        }
        else
        {
            _img.sprite = _sprite[1];
            _img.color = Color.red;
        }
    }
}
