using UnityEngine;

public class Tutorial_GrapplingRope : MonoBehaviour
{
    // 일반적인 참조 변수들
    [Header("General References:")]
    public Tutorial_GrapplingGun grapplingGun;
    public LineRenderer m_lineRenderer;

    // 일반적인 설정 변수들
    [Header("General Settings:")]
    [SerializeField] private int percision = 40; // 선의 정밀도
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5; // 직선으로 펴는 속도

    // 로프 애니메이션 설정 변수들
    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2; // 시작 웨이브 크기
    float waveSize = 0;

    // 로프 진행 설정 변수들
    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve; // 로프 진행 곡선
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1; // 로프 진행 속도
    
    float moveTime = 0;

    [HideInInspector] public bool isGrappling = true; // 그래플링 중인지 여부

    bool strightLine = true; // 직선인지 여부

    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.positionCount = percision;
        waveSize = StartWaveSize;
        strightLine = false;

        LinePointsToFirePoint();

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    // 로프의 끝점을 발사 지점으로 설정
    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    // 로프 그리기
    void DrawRope()
    {
        if (!strightLine)
        {
            if (m_lineRenderer.GetPosition(percision - 1).x == grapplingGun.grapplePoint.x)
            {
                strightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                grapplingGun.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }

    // 웨이브 형태 로프 그리기
    void DrawRopeWaves()
    {
        for (int i = 0; i < percision; i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }

    // 직선으로 로프 그리기
    void DrawRopeNoWaves()
    {
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }
}

