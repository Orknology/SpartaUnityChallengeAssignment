# 심화주차 개인과제

<img src="https://github.com/Orknology/SpartaUnityChallengeAssignment/assets/122108152/0afa54b7-9def-444f-9e79-0ed74d3ff86a" width="800" height="400">

## 프로젝트 소개
주요 주제 : **동적 생성** - 씬 전환, UI 생성 / 다른 강의따라서 기능 구현하는 시간

# 기능 설명

## 필수 요구

1. **씬 변경**
   
   1-1. 인트로 씬 구성
      - UI 구성
      - 시작버튼 추가
          - 시작 버튼시 씬 전환 - 메인 게임으로 이동
            
   1-2. 기본 기능
      - 캐릭터 이동
      - 카메라 이동
      - 방 만들기

 ## 추가 요구
  - Dictionary 활용
  - Queue, Stack 활용
  - raycast
  - 오브젝트 폴링
  - Instantiate 로 오브젝트 생성

# 주요 코드

**Managers**
  - 싱글톤으로 구현된 모든 Manager를 관리할 클래스.
  - 유일하게 Monobehavior를 상속받는 Manager.
  - Managers를 통해 다른 Manager에 접근가능.
    
    하위 Manager
    
        - UIManager - UI의 생성과 제거 - 스택
            
        - ResourceManager - 리소스 폴더에 프리팹들을 생성 및 제거 (Instantiate)
    
        - PoolManager - 오브젝트 생성 시 오브젝트 풀을 담당 - 큐 / 딕셔너리
    
        - SceneManager - 씬의 로드에 사용 (유니티 SceneManager 네이밍과 겹쳐서 AdditionalSceneManager
    
        - GameManager - 메인 씬의 게임 로직을 다루려고 했으나 현재 더미 데이터화
    
    *(SoundManager는 급조하느라 Manager에서 관리되지 않고 별개의 풀을 사용하고 있음)

**BaseScene**
  - 모든 씬 요소의 부모 클래스.
  - 이벤트 시스템을 생성하고 씬 타입을 받는다.
  - 이후 개별 씬 코드에서 해당 씬에서 필요한 것들을 리소스 매니저를 통해 생성 (씬 별 UI 포함)

**UI_Base**
  - 모든 UI 요소의 부모 클래스.
  - Bind를 이용해 Button, Text 등을 objects에 저장할 수 있다.
  - 각 씬 별로 필요한 것들을 담은 UI 코드가 있어 씬에서 불러온다. (ex = Intro씬의 타이틀 이미지 모션)

  - 그렇기에 상단의 사진에 과제 스타트 버튼은 인스펙터 창에서 아무 기능이 없지만 UI_Intro에서 MainScene으로 넘어가는 코드가 들어있어 작동한다.

  - **UI_EventHandler**
    - 마우스 포인터와 클릭 감지
      
**Util**
  - 여러 곳에서 쓰일 수 있는 static 클래스
  - 현재 Define과 Utilities가 존재
    

<img src="https://github.com/Orknology/SpartaUnityChallengeAssignment/assets/122108152/9a5dbbff-f458-4653-9486-040c8e0b98e8" width="800" height="400">


**Player**
  - 카메라 = player = GameObject.Find("Player");를 Start()에 사용하여 생성된 플레이어를 자동으로 따라감
    
  - 컨트롤러 = 플레이어 자식으로 있는 groundCheck가 isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);를 통해 바닥에 닿고 있는지 체크 (점프 제한)
    
  - 그래플링 = 유투브 강의를 통해 따라서 제작 해봄 - 라인 랜더러와 커브 / 스프링 조인트를 중심으로 구현 / 레이케스트를 통해 그래플 가능 여부 파악


    
 

