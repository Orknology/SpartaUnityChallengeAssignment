using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    // Pool 클래스 정의

    #region PoolRegion
    class Pool 
    {
        public GameObject Origin { get; private set; } // 원본 오브젝트
        public Transform Root { get; set; } // 풀의 루트 Transform
        private Queue<isPoolable> _poolQueue = new Queue<isPoolable>(); // 풀을 담을 큐
        
        // 풀 초기 설정
        public void Init(GameObject origin, int count)
        {
            Origin = origin;
            Root = new GameObject($"{origin.name}_Root").transform; // 루트 생성

            // 초기 개수만큼 풀에 객체 생성
            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        // 오브젝트 생성 및 풀에 넣는 메서드
        private isPoolable Create()
        {
            GameObject go = Object.Instantiate(Origin);
            go.transform.SetParent(Root);
            go.name = Origin.name;
            return go.GetOrAddComponent<isPoolable>(); // isPoolable 컴포넌트를 추가한 후 반환
        }

        // 풀에 반환
        public void Push(isPoolable _isPoolable)
        {
            if (_isPoolable == null)
                return;

            _isPoolable.transform.parent = Root;
            _isPoolable.gameObject.SetActive(false); // 비활성
            _isPoolable.IsUsing = false; // 사용 중이 아님
            _isPoolable.transform.position = new Vector3(300, 0, 0); // 임의의 위치로 이동
            
            _poolQueue.Enqueue(_isPoolable); // 큐에 추가
        }

        // 풀에서 가져오기
        public isPoolable Pop()
        {
            isPoolable _isPoolable;

            if (_poolQueue.Count > 0)
                _isPoolable = _poolQueue.Dequeue(); // 큐에서 꺼내옴
            else
                _isPoolable = Create(); // 없으면 새로 생성

            _isPoolable.gameObject.SetActive(true);
            _isPoolable.IsUsing = true;

            return _isPoolable; // 오브젝트 반환
        }
    }
    
    #endregion

    private Dictionary<string, Pool> _poolDict = new Dictionary<string, Pool>(); // 풀을 관리하는 딕셔너리
    private Transform _root; // 모든 풀의 부모가 될 루트 Transform

    // 초기 설정
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@Pool_Root").transform; // 풀 루트 생성
            Object.DontDestroyOnLoad(_root);
        }
    }

    // 풀 생성
    public void CreatePool(GameObject origin, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(origin, count); // 풀 초기 설정
        pool.Root.parent = _root; // 풀의 부모를 풀 루트로 설정

        _poolDict.Add(origin.name, pool); // 딕셔너리에 풀 추가
    }

    // 풀에 반환
    public void Push(isPoolable _isPoolable)
    {
        if (_poolDict.ContainsKey(_isPoolable.name) == false)
        {
            Object.Destroy(_isPoolable.gameObject); // 풀이 없으면 오브젝트 파괴
            return;
        }
        
        _poolDict[_isPoolable.name].Push(_isPoolable); // 오브젝트 반환
    }

    // 풀에서 가져오기
    public isPoolable Pop(GameObject origin, Transform parent = null)
    {
        if (_poolDict.ContainsKey(origin.name) == false)
            CreatePool(origin); // 풀이 없으면 새로 생성

        return _poolDict[origin.name].Pop(); // 풀에서 오브젝트 가져오기
    }

    // 오브젝트의 원본을 반환
    public GameObject GetOrigin(string name)
    {
        if (_poolDict.ContainsKey(name) == false)
            return null;

        return _poolDict[name].Origin;
    }

    // 모든 풀 초기화 및 삭제하는 메서드
    public void Clear()
    {
        foreach (Transform child in _root)
        {
            Object.Destroy(child.gameObject);
        }

        _poolDict.Clear(); // 딕셔너리 초기화
    }
}