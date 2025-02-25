using ExitGames.Client.Photon;
using Photon.Realtime;
using SpaceHub.Conference;
using System.Collections.Generic;
using UnityEngine;
using SpaceHub.Groups;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerKickManager : MonoBehaviour, IOnEventCallback
{
    [SerializeField] GameObject KickObj;
    [SerializeField] GameObject KickMessigeObj;
    [SerializeField] TMP_Text kickRoomName;

    enum PlayerKickMessge
    {
        Kick = 105,
        Temp,        
    }
    void Start()
    {
        PlayerLocal.Instance.Client.AddCallbackTarget(this);
    }
    private void OnDestroy()
    {
        if (PlayerLocal.Instance != null)
            PlayerLocal.Instance.Client.RemoveCallbackTarget(this);
    }

    public void KickMessige()
    {
        KickMessigeObj.SetActive(true);
        KickMessigeObj.GetComponent<PlayerKickObjScript>().PlayerKickMessigeBtn(
            (string)PlayerLocal.Instance.Client.CurrentRoom.CustomProperties["KickPlayerName"], 
            (string)PlayerLocal.Instance.Client.CurrentRoom.CustomProperties["KickPlayerId"]);
    }

    public void Kick(string playerId)
    {
        Dictionary<string, object> customContent = new Dictionary<string, object>();
        customContent.Add("kick", playerId);
        SendEvent(PlayerKickMessge.Kick, customContent);
    }

    void SendEvent(PlayerKickMessge code, object customContent)
    {
        RaiseEventOptions options = new RaiseEventOptions()
        {
            InterestGroup = 0,
            Receivers = ReceiverGroup.All
        };

        SendOptions sendOptions = new SendOptions()
        {
            DeliveryMode = DeliveryMode.Reliable
        };
        PlayerLocal.Instance.Client.OpRaiseEvent((byte)code, customContent, options, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch ((PlayerKickMessge)photonEvent.Code)
        {
            case PlayerKickMessge.Kick:
                var data = (Dictionary<string, object>)photonEvent.CustomData;
                var kick = data["kick"] as string;
                KickPlayer(kick);
                break;
            case PlayerKickMessge.Temp:

                break;
        }
    }

    void KickPlayer(string userId)
    {
        if (PlayerLocal.Instance.Client.UserId == userId)
        {
            if (PlayerLocal.Instance.Client.State == ClientState.Joined)
            {
                KickObj.SetActive(true);
                kickRoomName.text = ConferenceRoomManager.Instance.CurrentRoom.CreateRoomName + "\n강의실에서 퇴장 되었습니다.";
            }
        }
    }

    public void KickMessgeBtn()
    {
        ConferenceRoomManager.LoadRoom(ConferenceRoomManager.Instance.PreviousRoom);
    }
}
