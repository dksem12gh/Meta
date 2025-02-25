using ExitGames.Client.Photon;
using Photon.Realtime;
using SpaceHub.Conference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AccessorListPopup2 : Popup, IConnectionCallbacks, IMatchmakingCallbacks
{
    //rand
    //[SerializeField]
    //Button openBtn;

    [SerializeField]
    Sprite selectedBtnSprite;

    [SerializeField]
    Sprite normalBtnSprite;

    [SerializeField]
    ScrollRect userList;

    [SerializeField]
    AccessorListItem itemPrefab;

    private void Awake()
    {
        ConferenceVoiceConnection.Instance.Client.ConnectionCallbackTargets.Add(this);
        PlayerLocalConnector.Instance.Connector.JoinedRoomCallback += OnJoinedRoom;
    }
    private void OnDestroy()
    {
        ConferenceVoiceConnection.Instance.Client.ConnectionCallbackTargets.Remove(this);
        PlayerLocalConnector.Instance.Connector.JoinedRoomCallback -= OnJoinedRoom;
    }
    private void AddPlayer(Player player)
    {
        AccessorListItem item = Instantiate(itemPrefab, userList.content);
        item.UpdateUserInfo(player.UserId, player.NickName, player.GetJobNameProperty());
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

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayer(newPlayer);
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayer(otherPlayer);
    }

    IEnumerator PlayerRoomCheck()
    {
        yield return new WaitForSeconds(5.0f);

        StopAllCoroutines();
    }

    public void OnRoomLoding()
    {
        
    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster() 
    {
        if (PlayerLocal.Instance.Client.CurrentRoom != null)
        {
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
        }
    }

    public void OnDisconnected(DisconnectCause cause) { }

    public void OnRegionListReceived(RegionHandler regionHandler) { }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }

    public void OnCustomAuthenticationFailed(string debugMessage) { }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        if (PlayerLocal.Instance.VoiceConnection.Client.IsConnectedAndReady)
        {
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
        }
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new NotImplementedException();
    }
}