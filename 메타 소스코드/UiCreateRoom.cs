using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiCreateRoom : MonoBehaviour
{
    [SerializeField] TMP_InputField roomNameTmp;
    [SerializeField] TMP_InputField playerCountText;
    [SerializeField] TMP_InputField hourText;
    [SerializeField] TMP_InputField minText;
    [SerializeField] TMP_Text dayText;

    [SerializeField] Button roomCreateBtn;

    int hour = 0;
    int min = 0;
    public int playerCount = 0;
    string day = null;
    string roomName = null;

    public bool[] _check = new bool[3];

    private void OnEnable()
    {
        hour = 1;
        min = 0;
        playerCount = 200;
        day = "AM";
        roomName = null;
        roomCreateBtn.interactable = false;

        roomNameTmp.text = "숭실대학교 메타버스 스쿨";
        playerCountText.text = playerCount.ToString();
        hourText.text = hour.ToString();
        minText.text = min.ToString();
        dayText.text = day;

        for (int i = 0; i < 3; i++)
        {
            _check[i] = false;
        }
        _check[0] = true;
        _check[1] = true;
        _check[2] = true;
        RoomBtnCheck();
    }

    public void ChangePlayerCount()
    {
        _check[0] = true;

        string count = playerCountText.text;
        int.TryParse(count.Replace("u200B", ""),out playerCount);

        if (playerCount >= 250)
        {
            playerCount = 250;
            playerCountText.text = playerCount.ToString();
        }
        else if (playerCount == 0)
        {
            _check[0] = false;
            playerCount = 0;
            playerCountText.text = playerCount.ToString();
        }
        RoomBtnCheck();
    }

    public void PlayerCountUp()
    {
        _check[0] = true;

        playerCount++;

        if (playerCount >= 250)
        {
            playerCount = 250;
            playerCountText.text = playerCount.ToString();
            return;
        }

        playerCountText.text = playerCount.ToString();
        RoomBtnCheck();
    }

    public void PlayerCountDown()
    {
        playerCount--;

        if (playerCount <= 0)
        {
            _check[0] = false;
            playerCount = 0;
            playerCountText.text = playerCount.ToString();
            RoomBtnCheck();
            return;
        }
        _check[0] = true;

        playerCountText.text = playerCount.ToString();
        RoomBtnCheck();
    }//인원설정

    public void ChangeHour()
    {        
        string count = hourText.text;

        int.TryParse(count.Replace("u200B", ""), out hour);

        _check[2] = true;

        if (hour >= 12)
        {            
            hour = 12;
            hourText.text = hour.ToString();
        }
        else if (hour <= 0)
        {
            _check[2] = false;
            hour = 0;
            hourText.text = hour.ToString();
        }
        else if(count == null)
        {
            _check[2] = false;            
        }
        RoomBtnCheck();
    }

    public void HourCountUp()
    {
        hour++;
        
        if (hour >= 12)
        {
            hour = 12;
            hourText.text = hour.ToString();
            return;
        }

        hourText.text = hour.ToString();
        RoomBtnCheck();
    }

    public void HourCountDown()
    {
        hour--;
        
        if (hour <= 1)
        {
            hour = 1;            
        }

        hourText.text = hour.ToString();
        RoomBtnCheck();
    }//시간

    public void ChangeMin()
    {
        string count = minText.text;
        int.TryParse(count.Replace("u200B", ""), out min);

        if (min >= 59)
        {
            min = 59;
        }
        minText.text = min.ToString();
        RoomBtnCheck();
    }

    public void MinCountUp()
    {
        min++;

        if (min >= 59)
        {
            min = 59;
            minText.text = min.ToString();
            return;
        }

        minText.text = min.ToString();
        RoomBtnCheck();
    }

    public void MinCountDown()
    {
        min--;

        if (min <= 0)
        {
            min = 0;
            minText.text = min.ToString();
            return;
        }

        minText.text = min.ToString();
        RoomBtnCheck();
    }//분

    public void ChangeDay()
    {        
        if (dayText.text == "AM")
        {
            dayText.text = "PM";
            day = dayText.text;
        }
        else
        {
            dayText.text = "AM";
            day = dayText.text;
        }
    }

    public void ChangeRoomName()
    {
        string name = roomNameTmp.text.TrimStart();

        if (name.Length == 0)
        {
            _check[1] = false;            
            RoomBtnCheck();
            return;
        }
        _check[1] = true;
        roomName = roomNameTmp.text;
        RoomBtnCheck();

    }
    public void ChangeEndRoomName()
    {
        int count = roomNameTmp.text.Length;
        if (count != 0) return;            
        _check[1] = false;
        roomName = name;
        RoomBtnCheck();                    
    }

    void RoomBtnCheck()
    {
        if (_check[0] && _check[1] && _check[2])
        {
            roomCreateBtn.interactable = true;
        }
        else
            roomCreateBtn.interactable = false;
    }
}
