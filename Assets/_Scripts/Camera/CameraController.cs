using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineCamera _camera;
    private CinemachineBasicMultiChannelPerlin _cameraShake;

    private Vector3 _animalPlayerCenter;
    private GameObject _followTarget;
    private Vector3 _cameraOffset = new Vector3(5f, 0f, 0f);
    private Vector3 _targetCameraPosition;

    private float _zoomSpeed = 1f;
    private float _targetFOV = 2.5f;
    private float _originalFOV = 3f;
    private float _endOfMapFOV = 5f;

    [SerializeField]
    private Transform _endOfMap;

    private bool _hasZoomedOut = false;
    

    private void Awake()
    {
        _camera = GetComponent<CinemachineCamera>();
        _cameraShake = GetComponent<CinemachineBasicMultiChannelPerlin>();
        _followTarget = new GameObject("CameraFollowTarget");
        _camera.Follow = _followTarget.transform;
    }

    private void FixedUpdate()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        if (Player.Instance.transform.position.x >= _endOfMap.position.x)
        {
            ZoomOutAtEndOfMap();
        }
        else
        {
            HandleAnimalLassoedCamera();
            HandleCameraShake();
        }
    }

    private void HandleAnimalLassoedCamera()
    {        
        if (MinigameManager.Instance.GameWon)
        {
            _camera.Follow = null;
            ResetToDefaultZoom();
        }
        else if (LassoController.Instance.AnimalLassoed)
        {
            UpdateAnimalPlayerCenter();
            SetCameraToAnimalPlayerCenter();
            ZoomCameraToTargetFOV();
        }
        else
        {
            ResetToDefaultZoom();
            ResetFollowTarget();
        }
    }
    private void HandleEndMapZoom()
    {


    }
    private void ZoomOutAtEndOfMap()
    {
        if (!_hasZoomedOut)
        {
            _camera.Follow = null;
            _targetCameraPosition = _camera.transform.position + _cameraOffset;
            _hasZoomedOut = true;
        }

        float currentFOV = _camera.Lens.OrthographicSize;
        Vector3 currentPosition = _camera.transform.position;

        if (currentFOV < _endOfMapFOV)
        {
            currentFOV = Mathf.Lerp(currentFOV, _endOfMapFOV, _zoomSpeed * Time.deltaTime);
            _camera.Lens.OrthographicSize = Mathf.Min(currentFOV, _endOfMapFOV);
        }

        _camera.transform.position = Vector3.Lerp(currentPosition, _targetCameraPosition, _zoomSpeed * Time.deltaTime);
        

    }



    private void UpdateAnimalPlayerCenter()
    {
        _animalPlayerCenter = new Vector2((Player.Instance.transform.position.x + LassoController.Instance.CurrentLassoedAnimal.transform.position.x) / 2f, Player.Instance.transform.position.y);
        _followTarget.transform.position = _animalPlayerCenter;
    }

    private void SetCameraToAnimalPlayerCenter()
    {
        _camera.Follow = _followTarget.transform;
    }

    private void ZoomCameraToTargetFOV()
    {
        float orthographicSize = _camera.Lens.OrthographicSize;

        if (orthographicSize > _targetFOV)
        {
            orthographicSize -= _zoomSpeed * Time.deltaTime;
            _camera.Lens.OrthographicSize = Mathf.Max(orthographicSize, _targetFOV);
        }
    }

    private void ResetToDefaultZoom()
    {
        float orthographicSize = _camera.Lens.OrthographicSize;

        if (orthographicSize != _originalFOV)
        {
            orthographicSize = Mathf.Lerp(orthographicSize, _originalFOV, _zoomSpeed * Time.deltaTime);
            _camera.Lens.OrthographicSize = orthographicSize;
        }
    }

    private void ResetFollowTarget()
    {
        _camera.Follow = Player.Instance.transform;
    }

    private void HandleCameraShake()
    {
        if (MinigameManager.Instance.GameWon)
        {
            _cameraShake.AmplitudeGain = 0f;
        }
        else if (LassoController.Instance.AnimalLassoed)
        {
            _cameraShake.AmplitudeGain = 0.2f + (MinigameManager.Instance.FillAmount * 2);
        }
        else
        {
            _cameraShake.AmplitudeGain = 0f;
        }
    }
}
