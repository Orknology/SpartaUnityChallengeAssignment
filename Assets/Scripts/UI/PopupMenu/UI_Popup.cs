using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    // 그래픽 요소 배열
    private Graphic[] _graphics;

    // 초기설정
    protected override void Init()
    {
        // 팝업 UI의 Canvas 설정
        Managers.UI.SetCanvas(gameObject);

        // 포함된 모든 그래픽 요소들 가져오기
        _graphics = GetComponentsInChildren<Graphic>();
        // 각 그래픽 요소에 대해 페이드 인 효과 적용
        foreach (Graphic graphic in _graphics)
            StartCoroutine(FadeIn(graphic, 0, 1));
    }

    // 팝업 닫기
    public void ClosePopup()
    {
        Managers.UI.ClosePopupUI(this);
    }

    // 페이드 인 효과 적용하는 코루틴
    // 시간이 있었다면 DOTween에서 DOFade 기능이 있다는 걸 봤기에 이를 써보고 싶기는 했는데
    // 그것까지 공부하기에는 시간 부족...
    private IEnumerator FadeIn(Graphic graphic, float start, float end)
    {
        // "Blind"라는 이름을 가진 그래픽은 제외하고 바로 리턴
        if(graphic.name == "Blind")
            yield break;

        // 시간 및 퍼센트 초기화
        float current = 0;
        float percent = 0;
        // 페이드 인 시간
        float time = 0.1f;

        // 색상 가져오기
        Color color = graphic.color;
        // 시작 알파 값
        color.a = start;
        graphic.color = color;

        // 페이드 인 애니메이션 수행
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            // 알파 값 Lerp
            // 스탠더드 세션 때 강의 참고
            color.a = Mathf.Lerp(start, end, percent);
            graphic.color = color;

            yield return null;
        }
    }
}
