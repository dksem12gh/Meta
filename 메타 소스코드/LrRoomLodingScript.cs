using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using SpaceHub.Conference;

public class LrRoomLodingScript : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("SceneLoadReady");
    }

    IEnumerator SceneLoadReady()
    {
        yield return new WaitUntil( () => PlayerLocal.Instance.Client.IsConnectedAndReady);

        yield return new WaitForSeconds(3.0f);
        
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine("SceneLoadReady");
    }
}
