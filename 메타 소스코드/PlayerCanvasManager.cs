using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerCanvasManager : MonoBehaviour
{
    private static PlayerCanvasManager instance;
    public static PlayerCanvasManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerCanvasManager>();
            }
            return instance;
        }
    }

    public event Action<Popup> onPopup;
    public event Action<Popup> onClose;

    // Start is called before the first frame update
    public int PopupCount
    {
        get => popupList.Count;
    }

    List<Popup> popupList;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        popupList = new List<Popup>();
    }

    public void Popup(Popup popup)
    {
        if (popup == null)
        {
            return;
        }

        if (IsPrefab(popup))
        {
            popup = Object.Instantiate(popup);
        }

        int siblingIndex = popupList.Count;

        Transform popupTransform = popup.transform;
        popupTransform.SetParent(transform, false);
        popupTransform.SetSiblingIndex(siblingIndex);

        popup.gameObject.SetActive(true);

        popupList.Add(popup);

        onPopup?.Invoke(popup);
    }

    public void Close(Popup popup)
    {
        if (!popupList.Contains(popup))
        {
            return;
        }

        popup.gameObject.SetActive(false);
        popupList.Remove(popup);

        onClose?.Invoke(popup);
    }

    public void CloseAll()
    {
        while (popupList.Count > 0) 
        {
            Popup popup = popupList[0];
            Close(popup);
        }
    }

    public void Back()
    {
        Popup popup = popupList[popupList.Count - 1];
        Close(popup);
    }

    private bool IsPrefab(Component component)
    {
        return IsPrefab(component.gameObject);
    }

    private bool IsPrefab(GameObject go)
    {
        return (go.scene.name == null);
    }
}
