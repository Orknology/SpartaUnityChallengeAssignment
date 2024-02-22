using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    // 타입별 오브젝트를 저장하는 딕셔너리
    private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    
    private void Awake()
    {
        Init();
    }

    // 초기설정
    protected virtual void Init() { }

    // UI 오브젝트들을 바인딩하는 메서드
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // 해당 타입의 모든 이름 가져오기
        string[] names = Enum.GetNames(type);

        // 이름 수만큼의 배열 생성
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        // 모든 이름에 대해 오브젝트 검색 후 할당
        for (int i = 0; i < names.Length; i++)
        {
            // 타입이 GameObject인 경우, Utilities 클래스의 FindChild 메서드를 통해 검색
            if (type == typeof(GameObject))
                objects[i] = Utilities.FindChild(gameObject, names[i], true);
            // 그 외의 경우에는 컴포넌트 검색
            else
                objects[i] = Utilities.FindChild<T>(gameObject, names[i], true);
            
            if (objects[i] == null)
                Debug.Log($"바인드에 실패했습니다. : {type.Name}");
        }
    }

    // 바인딩 메서드 모음
    protected void BindButton(Type type) => Bind<Button>(type);
    protected void BindImage(Type type) => Bind<Image>(type);
    protected void BindText(Type type) => Bind<Text>(type);
    protected void BindInputField(Type type) => Bind<InputField>(type);
    protected void BindUIEventHandler(Type type) => Bind<UI_EventHandler>(type);
    protected void BindAnimator(Type type) => Bind<Animator>(type);

    // 특정 오브젝트를 가져오는 메서드
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects;
        // 해당 오브젝트 배열 가져오기
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            Debug.Log($"해당 키가 없습니다. : {typeof(T).Name}");
            return null;
        }
        
        return objects[index] as T;
    }

    // Get 모음
    protected Button GetButton(int index) => Get<Button>(index);
    protected Image GetImage(int index) => Get<Image>(index);
    protected Text GetText(int index) => Get<Text>(index);
    protected InputField GetInputField(int index) => Get<InputField>(index);
    protected UI_EventHandler GetUIEventHandler(int index) => Get<UI_EventHandler>(index);
    protected Animator GetAnimator(int index) => Get<Animator>(index);
}
