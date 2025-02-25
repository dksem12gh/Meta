using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using Photon.Voice.Unity;
using SpaceHub.Conference;
using SpaceHub.Groups;
using TMPro;
using Cysharp.Threading.Tasks;

/**
 * 2023-08-28
 * Dog : Change Class name Test to SceneUIInitializer.
 */
public class SceneUIInitializer : MonoBehaviour, IConnectionCallbacks ,IGroupUserIdProvider
{
    //test 클래스변경 , 
    /*
     * 모든 상황에 따른 ui 캔버스는 다 펼쳐져있음
     * 강의실 상황에 따라 캔버스 위치를 변경
     * 
     */

    [SerializeField]
    Toggle lectureRoomOverlayToggle;

    [SerializeField]
    Sprite selectedBtnSprite;

    [SerializeField]
    Sprite normalBtnSprite;

    [SerializeField]
    ScrollRect userList;

    [SerializeField]
    AccessorListItem itemPrefab_Professor;

    bool roomInvite;

    [SerializeField] Canvas LobbySceneCanvas;
    [SerializeField] Canvas LectureSceneCanvas;
    [SerializeField] GameObject LectureSceneOverlayRoot;
    [SerializeField] GameObject ClientDisconectCanvas;
    [SerializeField] Button[] lodingOverImg;

    [SerializeField] CharacterController _playerCC;
    [SerializeField] TMP_Text _roomName;



    [Obsolete]
    private void Awake()
    {
        LectureSceneCanvas.enabled = false;
        lectureRoomOverlayToggle.onValueChanged.AddListener(OnToggleLectureRoomOverlay);

        ConferenceVoiceConnection.Instance.Client.ConnectionCallbackTargets.Add(this);
        
        PlayerLocalConnector.Instance.Connector.JoinedRoomCallback += OnJoinedRoom;
        PlayerLocalConnector.Instance.Connector.LeftRoomCallback += OnLeftRoom;
        PlayerLocalConnector.Instance.Connector.DisconnectedCallback += OnDisconnected;
        roomInvite = false;
    }

    private void OnDestroy()
    {
        lectureRoomOverlayToggle.onValueChanged.RemoveListener(OnToggleLectureRoomOverlay);

        ConferenceVoiceConnection.Instance.Client.ConnectionCallbackTargets.Remove(this);        
        PlayerLocalConnector.Instance.Connector.JoinedRoomCallback -= OnJoinedRoom;
        PlayerLocalConnector.Instance.Connector.LeftRoomCallback -= OnLeftRoom;
        PlayerLocalConnector.Instance.Connector.DisconnectedCallback -= OnDisconnected;
    }

    private void AddPlayer(Player player)
    {
        Debug.Log("player add" + player.UserId);
        AccessorListItem item = Instantiate(itemPrefab_Professor, userList.content);
        item.UpdateUserInfo(player.UserId, player.NickName,player.GetJobNameProperty());
    }

    private void RemovePlayer(Player player)
    {
        foreach (Transform child in userList.content)
        {
            AccessorListItem item = child.GetComponent<AccessorListItem>();
            if (item.UserId == player.UserId)
            {
                Destroy(item.gameObject);
                return;
            }
        }
    }

    public void OnJoinedRoom()
    {        
        StartCoroutine("CanvasRoom");
    }

    public void OnLeftRoom()
    {
        //if (ConferenceRoomManager.Instance.PreviousRoom.SceneName == "lr_room")
        if (!ConferenceSceneSettings.Instance.IsLectureRoom) 
        {
            PlayerLocal.Instance.Connector.PlayerEnteredRoomCallback -= AddPlayer;
            PlayerLocal.Instance.Connector.PlayerLeftRoomCallback -= RemovePlayer;

            LobbySceneCanvas.enabled = true;
            LectureSceneCanvas.enabled = false;

            StartCoroutine("leaveChatRoom");            
        }
    }
    
    IEnumerator leaveChatRoom()
    {
        foreach (Transform child in userList.content)
        {
            Destroy(child.gameObject);
        }        

        if (roomInvite)
        {
            yield return new WaitForSeconds(4.0f);

            PlayerLocal.Instance.LeaveChatRoom(0);
            
            roomInvite = false;
        }
        
        StopCoroutine("leaveChatRoom");
    }

    IEnumerator CanvasRoom()
    {
        lectureRoomOverlayToggle.isOn = false;

        if (ConferenceSceneSettings.Instance.IsLectureRoom)
        {
            yield return new WaitUntil(() => ConferenceVoiceConnection.Instance.Client.IsConnectedAndReady);

            lectureRoomOverlayToggle.gameObject.SetActive(true);
            LectureSceneOverlayRoot.SetActive(false);

            LobbySceneCanvas.enabled = false;
            LectureSceneCanvas.enabled = true;

            _playerCC.radius = 0.001f;
            _playerCC.height = 1.7f;

            roomInvite = true;

            PlayerLocal.Instance.JoinChatRoom(0, "강의실-01");

            _roomName.text = ConferenceRoomManager.Instance.CurrentRoom.CreateRoomName;

            foreach (Transform child in userList.content)
            {
                Destroy(child.gameObject);
            }

            foreach (KeyValuePair<int, Player> playerInfo in PlayerLocal.Instance.Client.CurrentRoom.Players.ToArray())
            {
                AddPlayer(playerInfo.Value);
            }

            PlayerLocal.Instance.Connector.PlayerEnteredRoomCallback += AddPlayer;
            PlayerLocal.Instance.Connector.PlayerLeftRoomCallback += RemovePlayer;

            StopCoroutine("CanvasRoom");
        }
        else
        {
            lectureRoomOverlayToggle.gameObject.SetActive(false);

            LobbySceneCanvas.enabled = true;
            LectureSceneCanvas.enabled = false;

            _playerCC.radius = 0.22f;
            _playerCC.height = 1.7f;

            lodingOverImg[0].interactable = false;
            lodingOverImg[1].interactable = false;

            yield return new WaitForSeconds(4.0f);

            lodingOverImg[0].interactable = true;
            lodingOverImg[1].interactable = true;
        }
    }

    public void OnConnected()
    {}
    
    public void OnConnectedToMaster()
    {}

    public void OnCustomAuthenticationFailed(string debugMessage){}

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data){}

    public void OnDisconnected(DisconnectCause cause)
    {
        if (ClientDisconectCanvas == null) return;
        ClientDisconectCanvas.SetActive(true);        
    }

    public void OnRegionListReceived(RegionHandler regionHandler){}

    public string GetLocalUserId()
    {
        throw new NotImplementedException();
    }

    private void OnToggleLectureRoomOverlay(bool value)
    {
        LectureSceneOverlayRoot.SetActive(value);
    }
}
