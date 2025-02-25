using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using TMPro;
using SpaceHub.Conference;
using UnityEngine.UI;
using Rapa.UI;

public class RoomData : MonoBehaviour, IMatchmakingCallbacks
{
    [SerializeField] TextMeshProUGUI m_roomNameText = null;
    [SerializeField] TMP_InputField m_maxPlayerCountText = null;
    [SerializeField] ConferenceRoom m_conferenceRoom = new();
    [SerializeField] ToggleGroup m_roomToggleGroup = null;

    public ConferenceRoom enterRoomParams = new ConferenceRoom();
    public GameObject[] btn;
    public GameObject playerInputObj;

    ConferenceConnector m_connector = null;

    private void Start()
    {
        m_connector = PlayerLocalConnector.Instance.Connector;
                

    }

    public void ToggleRoomNameChange()
    {
        //switch(currentToggleRoomName)
    }

    public Toggle currentToggleRoomName
    {
        get { return m_roomToggleGroup.GetFirstActiveToggle(); }
    }

    public void RoomToggleChangeValueRoomEnter()
    {


        //int max = 0;
        //int.TryParse(m_maxPlayerCountText.text.Replace("u200B", ""), out max);
        byte maxByte = 255;        

        if (SpaceHub.Events.EventData.RawJsonNode != null &&
            SpaceHub.Events.EventData.RawJsonNode["maximum_players"] != null &&
            SpaceHub.Events.EventData.RawJsonNode["maximum_players"].AsInt >= 0 &&
            SpaceHub.Events.EventData.RawJsonNode["maximum_players"].AsInt <= 255)
        {
            maxByte = (byte)SpaceHub.Events.EventData.RawJsonNode["maximum_players"].AsInt;
        }
                        
        enterRoomParams.SceneName = "lr_room";               
        switch(currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text)
        {
            case "숭실대학교 메타버스 스쿨 01":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 02":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 03":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 04":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 05":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 06":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 07":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 08":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 09":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
            case "숭실대학교 메타버스 스쿨 10":
                enterRoomParams.CreateRoomName = currentToggleRoomName.GetComponent<ToggleTextProUGUI>().Text.text;
                break;
        }        
        
        btn[0].SetActive(true);
        btn[1].SetActive(true);
        btn[2].GetComponent<Canvas>().enabled = false;
        btn[3].GetComponent<Canvas>().enabled = false;

        ConferenceRoomManager.LoadRoom(enterRoomParams);
        btn[4].SetActive(true);
    }

    public void ViedioRoomJoin()
    {
        enterRoomParams.SceneName = "lr_room";

        btn[2].GetComponent<Canvas>().enabled = false;
        btn[3].GetComponent<Canvas>().enabled = false;

        ConferenceRoomManager.LoadRoom(enterRoomParams);
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedRoom()
    {

    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {             
    }
}
