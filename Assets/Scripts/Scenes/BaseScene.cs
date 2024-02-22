using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour
{
    public Define.Scenes SceneType { get; protected set; }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Managers.RM.Instantiate("UI/@EventSystem");
        }
    }
    
    public virtual void Clear() {}
}
