using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _camera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private Vector3 _animalAndPlayerCenterPoint;
    private GameObject _followTarget;
    private Vector3 _followOffset = new Vector3(2f, 0f, 0f);

    private float _zoomSpeed = 1f;
    private float _targetFOV = 2.5f;
    private float _originalFOV = 3f;

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
        _cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _followTarget = new GameObject("CameraFollowTarget");
        _camera.Follow = _followTarget.transform;
    }

    private void FixedUpdate()
    {
        HandleAnimalLassoedCamera();
        HandleCameraShakeOnLassoed();
    }

    private void HandleAnimalLassoedCamera()
    {
        if (MinigameManager.Instance.GameWon)
        {
            _camera.Follow = null;
            ManuallySetCameraPos();
            ReturnToOriginalFieldOfView();
        }
        else if (LassoController.Instance.AnimalLassoed)
        {
            FindAnimalAndPlayerCenter();
            ChangeCameraPositionOnLassoed();
            ZoomCameraOnLassoed();
        }
        else
        {
            UpdateFollowTarget();
            ReturnToOriginalFieldOfView();
            
        }
    }

    private void ManuallySetCameraPos()
    {
        Vector3 manualPosition = new Vector3(Player.Instance.playerPositionOnAnimalLassoed.x, (Player.Instance.playerPositionOnAnimalLassoed.y + 0.42f), _camera.transform.position.z);
        _camera.transform.position = manualPosition;
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
        if (MinigameManager.Instance.GameWon)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
        }
        else if (LassoController.Instance.AnimalLassoed)
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0.2f + (MinigameManager.Instance.FillAmount * 2);
        }
        else
        {
            _cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
        }
    }

    private void UpdateFollowTarget()
    {
        Vector3 offset = Vector3.zero; 

        if (Player.Instance.Rigidbody.linearVelocity.x > 0)
        {
            offset = _followOffset;
        }
        else if (Player.Instance.Rigidbody.linearVelocity.x < 0)
        {
            offset = -_followOffset;
        }

        Vector3 targetPosition = Player.Instance.transform.position + offset;
        _followTarget.transform.position = Vector3.Lerp(_followTarget.transform.position, targetPosition, Time.deltaTime * 2f);
    }
}
