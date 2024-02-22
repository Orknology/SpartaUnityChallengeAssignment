using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    //스택
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    // 현재 활성화된 UI
    private UI_Scene _scene = null;
    
    private GameObject Root
    {
        get
        {
            // @UI_Root 이름을 가진 GameObject 찾기
            GameObject root = GameObject.Find("@UI_Root");
            
            // 찾지 못한 경우 새로 생성하여 반환
            if (root == null)
            {
                root = new GameObject("@UI_Root");
                return root;
            }
            return root;
        }
    }

    // Canvas의 오더
    private int _order = 5;

    // Canvas 설정
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        // 가져온 캔버스 렌더링 모드를 ScreenSpaceOverlay로 설정
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 정렬 여부에 따라 오더 값 조절
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    // CoreUI
    public T MakeCore<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        // 이름이 주어지지 않은 경우 T의 이름을 사용
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // Resources에서 UI/CoreUI/하위에 있는 프리팹을 로드하여 인스턴스화
        GameObject go = Managers.RM.Instantiate($"UI/CoreUI/{name}");
        // 부모가 주어진 경우 해당 부모의 하위로 설정
        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    // SceneUI
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        
        GameObject go = Managers.RM.Instantiate($"UI/SceneUI/{name}");
        T scene = go.GetOrAddComponent<T>();
        _scene = scene;
        
        scene.transform.SetParent(Root.transform);

        return scene;
    }

    // PopupUI
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        
        GameObject go = Managers.RM.Instantiate($"UI/Popup/{name}");
        T popup = go.GetOrAddComponent<T>();
        _popupStack.Push(popup);        
        
        popup.transform.SetParent(Root.transform);

        return popup;
    }

    // Popup UI를 닫기
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        // 최상위 Popup이 아닌 경우 닫을 수 없음
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed.");
            return;
        }            

        ClosePopupUI();
    }

    // 최상위 Popup UI를 닫기
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        // 최상위 Popup 스택에서 꺼내어 파괴, 오더 값을 감소
        UI_Popup popup = _popupStack.Pop();
        Managers.RM.Destroy(popup.gameObject);        
        _order--;
    }

    // '모든' Popup UI를 닫기
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    // 모든 UI를 초기화
    public void Clear()
    {
        CloseAllPopupUI();
        _scene = null;
    }
}
