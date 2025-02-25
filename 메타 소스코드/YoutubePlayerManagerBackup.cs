using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using SpaceHub.Conference;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using UnityEngine.UI;
using TMPro;
using System;

public class YoutubePlayerManagerBackup : MonoBehaviour, IOnEventCallback
{
    public static YoutubePlayerManagerBackup Instance;
    public bool _Click = false;

    //[SerializeField] private YoutubePlayer.YoutubePlayer m_Player;
    [SerializeField] public VideoPlayer m_Player;
    [SerializeField] public Slider _YoutebeTimeLine;

    [SerializeField] GameObject[] _Btn;
    [SerializeField] Button _SoundMuteBtn;
    [SerializeField] Sprite[] _SountMuteBtnImg;
    [SerializeField] Button _SoundPaluseBtn;
    [SerializeField] Sprite[] _SountPaluseBtnImg;
    [SerializeField] Slider _SoundSlider;
    [SerializeField] TMP_Text[] _TimeText;

    string streamUrl = null;
    enum NetYoutubeEvent
    {
        Play = 101,
        Stop,
        Refresh,
        Join
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerLocal.Instance.Client.AddCallbackTarget(this);

        StartCoroutine("JopButtonCheck");

        if (m_Player.renderMode == VideoRenderMode.RenderTexture) 
        {
            m_Player.targetTexture.Release();
        }
    }

    IEnumerator JopButtonCheck()
    {
        yield return new WaitUntil(() => PlayerLocal.Instance.Client != null
            && PlayerLocal.Instance.Client.IsConnectedAndReady);

        if (PlayerLocal.Instance.Client.LocalPlayer.GetJobNameProperty() == "Professor")
        {
            if (_Btn.Length >= 3) 
            {
                _Btn[0].SetActive(true);
                _Btn[1].SetActive(true);
                _Btn[2].SetActive(true);
            }
        }
        else if (PlayerLocal.Instance.Client.LocalPlayer.GetJobNameProperty() == "Student")
        {
            if (_Btn.Length >= 3)
            {
                _Btn[0].SetActive(false);
                _Btn[1].SetActive(false);
                _Btn[2].SetActive(false);
            }

            if(_YoutebeTimeLine)
            {
                _YoutebeTimeLine.interactable = false;
            }
        }

        StopCoroutine("JopButtonCheck");
    }

    private void OnDestroy()
    {
        if (PlayerLocal.Instance.Client != null)
        {
            PlayerLocal.Instance.Client.RemoveCallbackTarget(this);
        }
    }

    private void FixedUpdate()
    {
        if(m_Player.isPlaying)
        {
            if (!_Click && _YoutebeTimeLine)
            {
                _YoutebeTimeLine.value = m_Player.frame;
            }

            if (_TimeText.Length > 0) 
            {
                double cur = m_Player.time;
                double max = m_Player.length;

                TimeSpan curTime = TimeSpan.FromSeconds(cur);
                TimeSpan maxTime = TimeSpan.FromSeconds(max);

                _TimeText[0].text = string.Format("{0:D2}:{1:D2}:{2:D2} / {3:D2}:{4:D2}:{5:D2}", curTime.Hours, curTime.Minutes, curTime.Seconds, maxTime.Hours, maxTime.Minutes, maxTime.Seconds);
            }
            //_TimeText[1].text = string.Format("{0:D2}:{1:D2}:{2:D2}", maxTime.Hours, maxTime.Minutes, maxTime.Seconds);
        }               
    }

    public void Join()
    {
        JoinRoom();
    }

    public void Play(string url)
    {
        var properties = PlayerLocal.Instance.Client.CurrentRoom.CustomProperties;

        if (!properties.ContainsKey("Youtube"))
        {
            properties.Add("Youtube", url);
        }
        else
        {            
            properties["Youtube"] = url;
        }
        
        PlayerLocal.Instance.Client.CurrentRoom.SetCustomProperties(properties);
        Dictionary<string, object> customContent = new Dictionary<string, object>();
        customContent.Add("url", url);
        SendEvent(NetYoutubeEvent.Play, customContent);
    }

    public void Refresh(string time)
    {
        Dictionary<string, object> customContent = new Dictionary<string, object>();
        customContent.Add("time", time);
        SendEvent(NetYoutubeEvent.Refresh, customContent);
    }

    public void Stop()
    {
        SendEvent(NetYoutubeEvent.Stop, null);
    }

    public void Pause()
    {
        m_Player.Pause();
    }

    void SendEvent(NetYoutubeEvent code, object customContent)
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
        switch ((NetYoutubeEvent)photonEvent.Code)
        {
            case NetYoutubeEvent.Play:
                var data = (Dictionary<string, object>)photonEvent.CustomData;
                var url = data["url"] as string;
                PlayYoutubue(url);
                streamUrl = url;
                break;
            case NetYoutubeEvent.Stop:
                StopYoutube();
                break;
            case NetYoutubeEvent.Refresh:
                var t = (Dictionary<string, object>)photonEvent.CustomData;
                var time = t["time"] as string;
                RefreshYoutebe(time);
                break;
            case NetYoutubeEvent.Join:                                
                break;
        }
    }

    async void PlayYoutubue (string url)
    {
        //url = "https://www.youtube.com/watch?v=b5qjG8fayDk";
        var client = new YoutubeClient();
        var manifest = await client.Videos.Streams.GetManifestAsync(url);

        var video = manifest.GetMuxedStreams().TryGetWithHighestVideoQuality();        

        m_Player.url = video.Url;        
        m_Player.Play();
        m_Player.SetDirectAudioVolume(0, 0.3f);
        StartCoroutine(YoutubeStarted());
    }

    void RefreshYoutebe(string time)
    {
        //_YoutebeTimeLine.value = float.Parse(time);
        m_Player.frame = (long)float.Parse(time);
        //m_Player.Play();
    }

    async void JoinRoom()
    {
        if (!PlayerLocal.Instance.Client.CurrentRoom.CustomProperties.ContainsKey("Youtube")) return;        

        string url = (string)PlayerLocal.Instance.Client.CurrentRoom.CustomProperties["Youtube"];       

        var client = new YoutubeClient();
        var manifest = await client.Videos.Streams.GetManifestAsync(url);
        var video = manifest.GetMuxedStreams().TryGetWithHighestVideoQuality();        

        m_Player.url = video.Url;     
        m_Player.Play();
        m_Player.SetDirectAudioVolume(0, 0.3f);
        StartCoroutine(YoutubeStarted());
    }

    public void StopYoutube()
    {
        if (m_Player.isPlaying) 
        {
            if (_SoundPaluseBtn) 
            {
                _SoundPaluseBtn.image.sprite = _SountPaluseBtnImg[1];
            }
            
            m_Player.Pause();
        }
        else
        {
            if (_SoundPaluseBtn)
            {
                _SoundPaluseBtn.image.sprite = _SountPaluseBtnImg[0];
            }

            m_Player.Play();
        }
        m_Player.targetTexture.Release();
    }

    public void YoutubeVolumCheck()
    {
        m_Player.SetDirectAudioVolume(0,_SoundSlider.value);


        if (_SoundSlider && _SoundSlider.value == 0)
        {
            if(_SoundMuteBtn)
            {
                _SoundMuteBtn.image.sprite = _SountMuteBtnImg[1];
            }
        }
        else
        {
            if (_SoundMuteBtn)
            {
                _SoundMuteBtn.image.sprite = _SountMuteBtnImg[0];
            }
        }

    }

    public void YoutebeMuteCheck()
    {
        if(!m_Player.GetDirectAudioMute(0))
        {
            m_Player.SetDirectAudioMute(0, true);

            if (_SoundMuteBtn)
            {
                _SoundMuteBtn.image.sprite = _SountMuteBtnImg[1];
            }
        }
        else if (m_Player.GetDirectAudioMute(0))
        {
            m_Player.SetDirectAudioMute(0, false);

            if (_SoundMuteBtn)
            {
                _SoundMuteBtn.image.sprite = _SountMuteBtnImg[0];
            }
        }
    }

    public void Seek(double frameIndex)
    {
        m_Player.time = frameIndex;        
    }

    public void YoutubeRemove()
    {
        m_Player.url = null;
        PlayerLocal.Instance.Client.CurrentRoom.CustomProperties.Remove("Youtube");
    }
    IEnumerator YoutubeStarted()
    {
        yield return new WaitUntil(() => m_Player.isPlaying);

        if (_YoutebeTimeLine) 
        {
            _YoutebeTimeLine.maxValue = m_Player.frameCount;
        }
    }
}
