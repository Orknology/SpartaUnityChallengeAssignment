using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // GameObject에서 자식 GameObject를 찾는 메서드
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        // FindChild<Transform> 메서드를 호출하여 Transform을 찾은 후 GameObject로 반환
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) 
            return null;

        return transform.gameObject;
    }

    // GameObject에서 특정 컴포넌트를 가진 자식을 찾는 메서드
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        // GameObject가 null인 경우 null 반환
        if (go == null)
            return null;

        // recursive가 false인 경우 부모의 직계 자식만 확인
        if (!recursive)
        {
            Transform transform = go.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                // name이 주어지지 않거나 자식의 이름이 일치하는 경우
                if (string.IsNullOrEmpty(name) || transform.GetChild(i).name == name)
                {
                    // 컴포넌트 타입 T를 찾아서 반환
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        // recursive가 true인 경우 모든 자식을 대상으로 확인
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // name이 주어지지 않거나 자식의 이름이 일치하는 경우 해당 컴포넌트 반환
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        // 찾지 못한 경우 null 반환
        return null;
    }

    // HUD(GameObject 이름이 "UI_Hud")를 기준으로 부모를 찾는 메서드
    public static Transform FindParentFromHUD(string name = null)
    {
        // HUD GameObject 찾기
        GameObject hud = GameObject.Find("UI_Hud");
        // HUD가 없는 경우 로그 출력 후 null 반환
        if(hud == null)
        {
            Debug.Log("현재 씬에 Hud가 존재하지 않습니다.");
            return null;
        }

        // HUD 아래에서 이름이 일치하는 Transform 찾기
        Transform parent = FindChild<Transform>(hud, name, true);
        // 찾지 못한 경우 로그 출력 후 null 반환
        if (parent == null)
        {
            Debug.Log($"해당 Transform이 존재하지 않습니다. : {name}");
            return null;
        }

        // 찾은 부모 Transform 반환
        return parent;
    }
}

