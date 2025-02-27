using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform patrolGroup;
    private Transform[] patrolPoints;
    private int currentPointIndex;
    public float patrolSpeed;

    public float alertDistance;
    public Transform coverPoint;

    public GameObject bloodPrefab;

    private bool isAlert;
    void Start()
    {
        patrolPointsArray();
        currentPointIndex = 0;
        isAlert = false;
        MoveToNextPatrolPoint();
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlert)
        {

        }
        else
        {

        }
    }

    void patrolPointsArray()
    {
        patrolPoints = new Transform[patrolGroup.childCount];
        for (int i = 0; i < patrolGroup.childCount; i++)
        {
            patrolPoints[i] = patrolGroup.GetChild(i);
        }
    }

    void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f) ;
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            MoveToNextPatrolPoint();
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alertDistance);
        foreach (Collider hitCo in hitColliders)
        {
            //만약 서클 내에 뭔가 있다면..
            //예시 if(hitCo.CompareTag("Player"))플레이어가 있다면
            //{isAlert= true;}
            //break; 찾았다면 반복빠져 나가기
        }
    }
    void MoveToNextPatrolPoint()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
    }
    void MoveToCover()
    {
        {
            transform.position = Vector3.MoveTowards(transform.position, coverPoint.position, patrolSpeed * Time.deltaTime);
            // 커버로 이동하는 애니메이션 등 설정
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            ContactPoint contactPoint = collision.GetContact(0);
            Vector3 hitPosition = contactPoint.point;
            Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);
            var Blood = Instantiate(bloodPrefab, hitPosition, rotation);
            Blood.transform.parent = transform;
        }
    }
    void HeadShot()
    {
        // 데스 애니메이션
        Debug.Log("Head shot!");
    }
    void BodyShot()
    {
        // 기어다니는 애니메이션
        Debug.Log("Body shot!");
    }
}
