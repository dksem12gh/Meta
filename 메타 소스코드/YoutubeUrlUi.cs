using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceHub.Conference;
using TMPro;
public class YoutubeUrlUi : MonoBehaviour
{
    public YoutubePlayerManagerBackup ypm = null;
    public TMP_InputField m_url = null;
    public GameObject btn = null;

    private void Start()
    {
        PlayerLocalConnector.Instance.Connector.LeftRoomCallback += OnLeftRoom;
    }

    private void OnDestroy()
    {
        PlayerLocalConnector.Instance.Connector.LeftRoomCallback -= OnLeftRoom;
    }
    public void OnLeftRoom()
    {
        if (ConferenceRoomManager.Instance.PreviousRoom.SceneName == "lr_room")
        {
            ypm.Stop();
        }
    }
    public void urlChange()
    {
        if (m_url.text.Trim() == "") return;

        if (CustomizationData.Instance.JobName == "Professor")
        {
            ypm.Play(m_url.text.Trim());            

            m_url.text = "";
            //btn.SetActive(false);
            ypm.gameObject.SetActive(true);
        }
    }

    public void urlTimeChange()
    {
        ypm._Click = false;
        ypm.Refresh(ypm._YoutebeTimeLine.value.ToString());
    }

    public void urlTimeBtnChange()
    {
        ypm.Refresh(ypm._YoutebeTimeLine.value.ToString());
    }

    public void urlTimeUp()
    {
        ypm._Click = true;
    }

    public void urlChangeIng()
    {
        if (CustomizationData.Instance.JobName == "Professor")
        {
            ypm.gameObject.SetActive(false);
        }
    }
    
    public void ypmStop()
    {
        ypm.Stop();        
    }
}