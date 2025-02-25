using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using SpaceHub.Conference;
using SpaceHub.Groups;

public class PlayerMuteManager : MonoBehaviour, IOnEventCallback
{
    [SerializeField] MediaControllableStackItem _mediaCtrl;
    GroupsManager m_GroupsManager => m_GroupsView.GroupsManager;
    [SerializeField] GroupsViewManager m_GroupsView;

    IVoiceGroupConnection m_VoiceConnection;

    enum PlayerMuteMessge
    {
        Mute = 107,        
        Speak
    }

    IEnumerator Start()
    {
        do
        {
            yield return null;
            PlayerLocal.Instance.Client.AddCallbackTarget(this);
            m_VoiceConnection = m_GroupsManager.GetConnection<IVoiceGroupConnection>();
        }
        while (m_VoiceConnection == null) ;
    }

    private void OnDestroy()
    {
        if (PlayerLocal.Instance != null)
            PlayerLocal.Instance.Client.RemoveCallbackTarget(this);
    }

    public void Mute(string playerId)
    {
        Dictionary<string, object> customContent = new Dictionary<string, object>();
        customContent.Add("mute", playerId);
        SendEvent(PlayerMuteMessge.Mute, customContent);
    }

    void SendEvent(PlayerMuteMessge code, object customContent)
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
        switch ((PlayerMuteMessge)photonEvent.Code)
        {
            case PlayerMuteMessge.Mute:
                var data = (Dictionary<string, object>)photonEvent.CustomData;
                var mute = data["mute"] as string;
                MutePlayer(mute);
                break;
            case PlayerMuteMessge.Speak:
                break;
        }
    }

    void MutePlayer(string userId)
    {
        if (PlayerLocal.Instance.Client.UserId == userId)
        {
            if (!m_VoiceConnection.GetMutedSelf())
            {
                m_VoiceConnection.SetMutedSelf(true);
            }
            else if (m_VoiceConnection.GetMutedSelf())
            {
                m_VoiceConnection.SetMutedSelf(false);
            }
        }
    }
}
