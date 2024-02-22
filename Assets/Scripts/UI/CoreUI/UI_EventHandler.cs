using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Pointer 이벤트
    
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData> OnClickHandler = null;

    public void OnPointerEnter(PointerEventData eventData) => OnEnterHandler?.Invoke(eventData);    

    public void OnPointerExit(PointerEventData eventData) => OnExitHandler?.Invoke(eventData);

    public void OnPointerClick(PointerEventData eventData) => OnClickHandler?.Invoke(eventData);
    
    // Input 액션
    
    public Action OnEnterInvoke;
    public Action OnEscInvoke;    

    private void Update()
    {
        HandleKeyInput(KeyCode.Return, OnEnterInvoke);
        HandleKeyInput(KeyCode.Escape, OnEscInvoke);
    }

    private void HandleKeyInput(KeyCode key, Action action)
    {
        if (Input.GetKeyDown(key))
            action?.Invoke();
    }
}