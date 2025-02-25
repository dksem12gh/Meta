using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerRoom : MonoBehaviour
{
    Transform m_Transform;
    [SerializeField] float Speed = 1f;

    private void Awake()
    {
        m_Transform = GetComponent<RectTransform>();
    }

    void Update()
    {
        m_Transform.rotation = Quaternion.Euler(0, 0, -Time.realtimeSinceStartup * 360f * Speed);
    }
}
