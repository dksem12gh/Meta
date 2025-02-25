using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerKickObjScript : MonoBehaviour
{
    public TMP_Text text = null;
    PlayerKickManager pkManager;

    string _userId;

    private void Awake()
    {
        if (pkManager == null)
        {
            pkManager = GameObject.Find("PlayerKickManager").GetComponent<PlayerKickManager>();
        }
    }

    public void PlayerKickMessigeBtn(string name , string userId)
    {
        text.text = name + "\n해당 플레이어를 퇴장 시키겠습니까?";
        _userId = userId;
    }

    public void KickBtn()
    {
        if (_userId == null) return;
        pkManager.Kick(_userId);
    }

    public void ClientExit()
    {
        SceneManager.LoadScene("Funnel", LoadSceneMode.Single);

/*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif*/
    }
}

