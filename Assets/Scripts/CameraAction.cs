using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CameraAction : MonoBehaviour
{
    [Header("카메라")]
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private CinemachineVirtualCamera _zoomInCam;

    [Header("크로스헤어 이미지")]
    [SerializeField] private RawImage _crossHair;

    [Header("카메라 줌/조작 설정")]
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 40f;
    [SerializeField] private float _minZoomRatio = 0.1f; // 최소 줌 비율
    [SerializeField] private float _zoomSpeed = 10f;

    [Header("플레이어")]
    [SerializeField] private Transform _playerTR;

    private PlayerActions action;
    private CinemachineComposer camComposer;
    private CinemachineComposer zoomInCamComposer;
    private CinemachineTransposer zoomInCamTr;
    private float vFov;

    void Start()
    {
        action = _playerTR.GetComponent<PlayerActions>();
        _crossHair.gameObject.SetActive(false);
        _cam.Priority = 10;
        _zoomInCam.Priority = 0;
        vFov = _cam.m_Lens.FieldOfView; // 기본 FOV 저장
        camComposer = _cam.GetCinemachineComponent<CinemachineComposer>();
        zoomInCamComposer = _zoomInCam.GetCinemachineComponent<CinemachineComposer>();
        zoomInCamTr = _zoomInCam.GetCinemachineComponent<CinemachineTransposer>();
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (!Input.GetMouseButton(1))
        {// 마우스 오른쪽 버튼을 누르지 않았다면(가만히 있는 상태)
            NormalHandler(mouseX, mouseY);
        }
        if (Input.GetMouseButtonDown(1))
        {// 마우스 오른쪽 버튼이 눌리면 1회 실행
            EnterAimMode();
        }
        if (Input.GetMouseButton(1))
        {// 마우스 오른쪽 버튼이 눌려있다면?(조준상태 진입)
            HandleAimViewControl(mouseX, mouseY, scrollData);   
        }        
        if (Input.GetMouseButtonUp(1))
        {// 마우스 오른쪽 버틴이 떼지면 1회 실행
            ExitAimMode();            
        }
    }

    private void NormalHandler(float mouseX, float mouseY)
    {
        camComposer.m_TrackedObjectOffset.y += mouseY;
        _playerTR.Rotate(0, mouseX, 0);
    }
    private void EnterAimMode()
    {
        action.ChangeState(PlayerState.AIMING);
        zoomInCamComposer.m_TrackedObjectOffset.x = camComposer.m_TrackedObjectOffset.x;
        zoomInCamComposer.m_TrackedObjectOffset.y = camComposer.m_TrackedObjectOffset.y;
    }
    private void HandleAimViewControl(float mouseX, float mouseY, float scrollData)
    {
        _crossHair.gameObject.SetActive(true);
        _cam.Priority = 0;
        _zoomInCam.Priority = 20;

        float zoomRatio = Mathf.Clamp((_zoomInCam.m_Lens.FieldOfView - _minZoom) / (_maxZoom - _minZoom), _minZoomRatio, 1.0f);
        float dynamicZoomSpeed = _zoomSpeed * zoomRatio;

        _zoomInCam.m_Lens.FieldOfView -= (scrollData * dynamicZoomSpeed);
        _zoomInCam.m_Lens.FieldOfView = Mathf.Clamp(_zoomInCam.m_Lens.FieldOfView, _minZoom, _maxZoom);

        mouseX *= zoomRatio * 0.5f;
        mouseY *= zoomRatio * 0.5f;

        zoomInCamComposer.m_TrackedObjectOffset.x += mouseX;
        zoomInCamComposer.m_TrackedObjectOffset.y += mouseY;
    }
    private void ExitAimMode()
    {
        _crossHair.gameObject.SetActive(false);
        camComposer.m_TrackedObjectOffset.x = 0f;
        camComposer.m_TrackedObjectOffset.y = 0f;
        zoomInCamComposer.m_TrackedObjectOffset.x = 0;
        zoomInCamComposer.m_TrackedObjectOffset.y = 0;
        _cam.Priority = 10;
        _zoomInCam.Priority = 0;
        _zoomInCam.m_Lens.FieldOfView = vFov;
    }
}