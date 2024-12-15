using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _camera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private Vector3 _animalAndPlayerCenterPoint;
    private GameObject _followTarget;

    private float _zoomSpeed = 1f;
    private float _targetFOV = 2.5f;
    private float _originalFOV = 3f;

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _followTarget = new GameObject("CameraFollowTarget");
    }

    private void FixedUpdate()
    {
        HandleAnimalLassoedCamera();
        HandleCameraShakeOnLassoed();
    }

    private void HandleAnimalLassoedCamera()
    {
        if (LassoController.Instance.AnimalLassoed)
        {
            FindAnimalAndPlayerCenter();
            ChangeCameraPositionOnLassoed();
            ZoomCameraOnLassoed();
        }
        else
        {
            _camera.Follow = Player.Instance.transform;
        }
    }

    private void FindAnimalAndPlayerCenter()
    {
        _animalAndPlayerCenterPoint = (Player.Instance.transform.position + LassoController.Instance.CurrentLassoedAnimal.transform.position) / 2f;
        _followTarget.transform.position = _animalAndPlayerCenterPoint;
    }

    private void ChangeCameraPositionOnLassoed()
    {
        _camera.Follow = _followTarget.transform;
    }

    private void ZoomCameraOnLassoed()
    {
        float orthographicSize = _camera.Lens.OrthographicSize;

        if (orthographicSize > _targetFOV)
        {
            orthographicSize -= _zoomSpeed * Time.deltaTime;
            _camera.Lens.OrthographicSize = Mathf.Max(orthographicSize, _targetFOV);
        }
    }

    private void ReturnToOriginalFieldOfView()
    {
        float orthographicSize = _camera.Lens.OrthographicSize;

        if (orthographicSize < _originalFOV)
        {
            orthographicSize += _zoomSpeed * Time.deltaTime;
            _camera.Lens.OrthographicSize = Mathf.Min(orthographicSize, _originalFOV);
        }
    }

    private void HandleCameraShakeOnLassoed()
    {
        if (LassoController.Instance.AnimalLassoed)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0.2f + (MinigameManager.Instance.FillAmount * 2);
        }
        else
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
        }
    }
}
