using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Intro : UI_Scene
{
    // 애니메이션 곡선
    [SerializeField] AnimationCurve curve;

    // 버튼 및 이미지 열거형 정의

    #region ENUM

    enum Buttons
        {
            Start_Btn,
        }
    
        enum Images
        {
            Title_Image,
        }

    #endregion
    

    // 초기설정
    protected override void Init()
    {
        base.Init();

        // 버튼, 이미지 바인딩
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        // 시작 버튼에 '클릭 리스너' 추가하여 메인 씬으로 전환
        GetButton((int)Buttons.Start_Btn).onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scenes.Main));

        // 타이틀 이미지에 바운스 애니메이션
        StartCoroutine(BounceRoutine(Vector3.one, Vector3.one * 0.9f));
    }

    // 바운스 애니메이션 코루틴 - 바운스 : 꿀렁꿀렁 이펙트(...)
    private IEnumerator BounceRoutine(Vector3 startSize, Vector3 endSize)
    {
        float current = 0;
        float percent = 0;

        // 타이틀 이미지 가져오기(인트로 이미지)
        Image titleImage = GetImage((int)Images.Title_Image);        
        
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 1; // 1초 동안 진행

            // Curve를 이용한 Lerp
            titleImage.transform.localScale = Vector3.Lerp(startSize, endSize, curve.Evaluate(percent));

            yield return null;
        }
        StartCoroutine(BounceRoutine(endSize, startSize));
    }
}

