using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam3D;
    [SerializeField] private CinemachineVirtualCamera _virtualCam2D;

    private Camera _mainCamera;
    private void Start() {
        _mainCamera = Camera.main;
    }

    private void OnEnable() {
        GameManager.OnChangeGameStyle += ChangeCam;
    }
    private void OnDisable() {
        GameManager.OnChangeGameStyle -= ChangeCam;
    }

    public void ChangeCam(GameStyle style)
    {
        if(_virtualCam3D == null || _virtualCam2D == null)
            return;

        switch (style)
        {
            case GameStyle.STYLE3D:
                _virtualCam3D.Priority = 12;
                _virtualCam2D.Priority = 6;

                StartCoroutine(TurnOrtographicDelay(false));
            break;

            case GameStyle.STYLE2D:
                _virtualCam3D.Priority = 6;
                _virtualCam2D.Priority = 12;

                StartCoroutine(TurnOrtographicDelay(true));
            break;
        }
    }

    private IEnumerator TurnOrtographicDelay(bool condition)
    {
        yield return new WaitForSeconds(1.5f);
        _mainCamera.orthographic = condition;
    }

}
