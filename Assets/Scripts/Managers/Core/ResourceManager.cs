using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    //리소스 로드
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        //T가 게임오브젝트 일 때
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOrigin(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    //리소스 인스턴시에이트
    public GameObject Instantiate(string path, Transform parent = null)
    {
        //게임 오브젝트 경로
        GameObject origin = Load<GameObject>($"Prefabs/{path}");

        if (origin == null)
        {
            Debug.Log($"오브젝트 찾기 실패 : {path}");
            return null;
        }
        //풀링할 프리팹에 달아줄 코드를 일종의 태그로 사용
        if (origin.GetComponent<isPoolable>() != null)
            return Managers.Pool.Pop(origin, parent).gameObject;
        
        //오브젝트 생성
        GameObject go = Object.Instantiate(origin, parent);
        go.name = origin.name;

        return go;
    }
    
    //게임 오브젝트 깔끔히 제거 - 오브젝트 풀 때문에 필요
    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        isPoolable _isPoolable = go.GetComponent<isPoolable>();
        if (_isPoolable != null)
        {
            Managers.Pool.Push(_isPoolable);
            return;
        }

        Object.Destroy(go);
    }
    
}
