using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;
using UnityEngine.UI;

public class CameraAction : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera zoomInCam;
    public PlayerMove player;
    [SerializeField] public float turnSpeed = 10.0f;    
    private float rotationSmoothTime = 0.1f;  // 부드러움 시간
    private float currentRotationVelocity;  // 현재 회전 속도

    private float vFov;
    private float targetFOV; // 목표 FOV 값
    [SerializeField] public float minZoom = 5f;
    [SerializeField] public float maxZoom = 40f;
    [SerializeField] public RawImage crossHair;
    [SerializeField] public float minZoomRatio = 0.1f; // 최소 줌 비율
    [SerializeField] public float zoomSpeed = 10f;

    private CinemachineComposer camComposer;
    private CinemachineComposer zoomInCamComposer;
    void Start()
    {          
        crossHair.gameObject.SetActive(false);
        cam.Priority = 10;
        zoomInCam.Priority = 0;
        vFov = cam.m_Lens.FieldOfView; // 기본 FOV 저장        
    }

    void Update()
    {
        camComposer = cam.GetCinemachineComponent<CinemachineComposer>();
        zoomInCamComposer = zoomInCam.GetCinemachineComponent<CinemachineComposer>();
        float rX = Input.GetAxis("Mouse X");
        float rY = Input.GetAxis("Mouse Y");
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (!Input.GetMouseButton(1))
        {
            camComposer.m_TrackedObjectOffset.x += rX;
            camComposer.m_TrackedObjectOffset.y += rY;
            float targetRotation = player.transform.eulerAngles.y + rX * turnSpeed;
            float smoothRotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotation, ref currentRotationVelocity, rotationSmoothTime);
                        
            player.transform.rotation = Quaternion.Euler(0, smoothRotation, 0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            zoomInCamComposer.m_TrackedObjectOffset.x = camComposer.m_TrackedObjectOffset.x;
            zoomInCamComposer.m_TrackedObjectOffset.y = camComposer.m_TrackedObjectOffset.y;
        }
        if (Input.GetMouseButton(1))
        {            
            crossHair.gameObject.SetActive(true);
            cam.Priority = 0;
            zoomInCam.Priority = 20;

            float zoomRatio = Mathf.Clamp((zoomInCam.m_Lens.FieldOfView - minZoom) / (maxZoom - minZoom), minZoomRatio, 1.0f);
            float dynamicZoomSpeed = zoomSpeed * zoomRatio;

            zoomInCam.m_Lens.FieldOfView -= scrollData * zoomSpeed;
            zoomInCam.m_Lens.FieldOfView = Mathf.Clamp(zoomInCam.m_Lens.FieldOfView, minZoom, maxZoom);
                        
            rX *= zoomRatio * 0.5f;
            rY *= zoomRatio * 0.5f;

            zoomInCamComposer.m_TrackedObjectOffset.x += rX;
            zoomInCamComposer.m_TrackedObjectOffset.y += rY;
        }
        // 마우스 오른쪽 버튼에서 손을 뗐을 때
        if (Input.GetMouseButtonUp(1))
        {
            crossHair.gameObject.SetActive(false);
            camComposer.m_TrackedObjectOffset.x = 0f;
            camComposer.m_TrackedObjectOffset.y = 0f;
            zoomInCamComposer.m_TrackedObjectOffset.x = 0;
            zoomInCamComposer.m_TrackedObjectOffset.y = 0;
            cam.Priority = 10;
            zoomInCam.Priority = 0;
            zoomInCam.m_Lens.FieldOfView = vFov;
        }        
    }
}