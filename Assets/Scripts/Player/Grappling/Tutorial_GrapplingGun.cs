//https://www.youtube.com/watch?v=dnNCVcVS6uw

using UnityEngine;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false; // 그래플링을 모든 레이어에 사용할지 여부
    [SerializeField] private int grappableLayerNumber = 9; // 그래플링 가능한 레이어 번호

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot; 
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true; // 시간에 따라 회전할지 여부
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4; // 회전 속도

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false; // 최대 거리를 사용할지 여부
    [SerializeField] private float maxDistnace = 20; // 최대 거리
    
    // 발사 타입
    private enum LaunchType
    {
        Transform_Launch, // 위치 기반 발사
        Physics_Launch // 물리 기반 발사
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true; // 특정 지점으로 발사할지 여부
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch; // 발사 타입
    [SerializeField] private float launchSpeed = 1; // 발사 속도

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false; // 거리를 자동으로 구성할지 여부
    [SerializeField] private float targetDistance = 3; // 목표 거리
    [SerializeField] private float targetFrequncy = 1; // 목표 주파수

    [HideInInspector] public Vector2 grapplePoint; // 그래플링 지점
    [HideInInspector] public Vector2 grappleDistanceVector; // 그래플링 거리 벡터
    private void Start()
    {
        // 그래플링 로프 및 스프링 조인트 비활성화
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        m_camera = Camera.main;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼을 누르면 그래플링 지점 설정
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        // 마우스 왼쪽 버튼을 누르고 있는 동안 총기 회전 및 발사
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        // 마우스 왼쪽 버튼을 뗄 때 그래플링 해제
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        // 그 외의 경우 총기 회전
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }

    // 총기 회전 메서드
    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // 그래플링 지점 설정 메서드
    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                }
            }
        }
    }

    // 그래플링 메서드
    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }

            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_springJoint2D.distance = distanceVector.magnitude;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    // 디버그 시, 최대 거리를 시각적으로 표시하는 메서드
    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }
}


