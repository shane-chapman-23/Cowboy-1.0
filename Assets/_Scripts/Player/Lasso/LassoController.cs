using System.Collections.Generic;
using UnityEngine;

public class LassoController : MonoBehaviour
{
    public static LassoController Instance {  get; private set; }

    //Player
    [SerializeField]
    private Player _player;
    //Lasso
    [SerializeField]
    private GameObject _lasso;
    [SerializeField]
    private Transform _lassoThrowStart;
    [SerializeField]
    private Transform _lassoThrownAnchor;
    [SerializeField]
    private Transform _animalLassoedLineAnchor;
    [SerializeField]
    private LassoPixelPoolManager _lassoPixelPool;

    private GameObject _lassoInstance;
    private Lasso _lassoScript;
    private Rigidbody2D _lassoRigidbody;
    private Vector3 _lassoLineAnchor;
    private List<GameObject> _lassoPixelInstances = new List<GameObject>();

    //Lassoed Animal
    private GameObject _currentLassoedAnimal;
    private Vector3 _animalLassoAnchor;

    private AnimatorStateInfo _currentAnimStateInfo;

    private bool _lassoSpawned;
    private bool _animalLassoed;

    private float _throwForce = 4f;
    private float _returnForce = 20f;
    private float _pixelsPerUnit = 16f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleLassoAnimationSync();
    }

    private void LateUpdate()
    {
        HandlePixelPerfectLine();
    }

    private void FixedUpdate()
    {
        HandleInstantiateAndThrowLasso();
        HandleReturnLasso();
        HandleDestroyLassoOnReturn();
        
    }

    #region Lasso Animations
    private void HandleLassoAnimationSync()
    {
        _currentAnimStateInfo = _player.Anim.GetCurrentAnimatorStateInfo(0);

        Dictionary<int, string> normalToLassoMap = new Dictionary<int, string>
        {
            { Animator.StringToHash("Idle"), "LassoIdle"},
            { Animator.StringToHash("Trot"), "LassoTrot" },
            { Animator.StringToHash("Gallop"), "LassoGallop"}
        };

        Dictionary<int, string> lassoToNormalMap = new Dictionary<int, string>
        {
            { Animator.StringToHash("LassoIdle"), "Idle"},
            { Animator.StringToHash("LassoTrot"), "Trot" },
            { Animator.StringToHash("LassoGallop"), "Gallop"}
        };

        if ((_player.InputHandler.LassoInputDown || _player.InputHandler.LassoInputUp) && !_lassoSpawned)
        {
            bool isPressed = _player.InputHandler.LassoInputDown;
            ToggleLassoAnimation(isPressed, normalToLassoMap, lassoToNormalMap);
        }
    }

    private void ToggleLassoAnimation(bool isPressed, Dictionary<int, string> normalToLassoMap, Dictionary<int, string> lassoToNormalMap)
    {
        _player.Anim.SetBool("lasso", isPressed);

        if (isPressed)
        {
            SwitchAnimation(normalToLassoMap);
        }
        else
        {
            SwitchAnimation(lassoToNormalMap);
        }
    }

    private void SwitchAnimation(Dictionary<int, string> animationMap)
    {
        if (animationMap.ContainsKey(_currentAnimStateInfo.shortNameHash))
        {
            _player.Anim.Play(animationMap[_currentAnimStateInfo.shortNameHash], 0, _currentAnimStateInfo.normalizedTime);
        }
    }

    #endregion

    #region Handle Lasso Functions
    private void HandleInstantiateAndThrowLasso()
    {
        if (_player.InputHandler.LassoInputUp && !_lassoSpawned)
        {
            InstantiateLasso();
            ThrowLasso();

            _player.InputHandler.SetLassoInputUpFalse();
        }
    }

    private void HandleReturnLasso()
    {
        if (_lassoSpawned && _lassoScript.isGrounded)
        {
            ReturnLasso();
        }
    }

    private void HandleDestroyLassoOnReturn()
    {
        float distanceToPlayer = Vector2.Distance(_lassoInstance.transform.position, _lassoThrownAnchor.position);

        if (_lassoSpawned && distanceToPlayer < 0.1f)
        {
            DestroyLasso();
            _lassoSpawned = false;
        }
    }
    #endregion

    #region Lasso Functions
    private void InstantiateLasso()
    {
        _lassoInstance = Instantiate(_lasso, _lassoThrowStart.position, transform.rotation);
        _lassoScript = _lassoInstance.GetComponent<Lasso>();
        _lassoRigidbody = _lassoInstance.GetComponent<Rigidbody2D>();
        _lassoSpawned = true;
    }

    private void ThrowLasso()
    {
        _lassoRigidbody.AddForce((Vector2.right * _player.facingDirection) * (_throwForce + _player.Rigidbody.linearVelocity.magnitude), ForceMode2D.Impulse);
    }

    private void ReturnLasso()
    {
        _lassoInstance.transform.position = Vector2.MoveTowards(
            _lassoInstance.transform.position,
            _lassoThrownAnchor.transform.position,
            _returnForce * Time.deltaTime
            );
    }

    private void DestroyLasso()
    {
        Destroy(_lassoInstance);
        _lassoInstance = null;
    }
    #endregion

    #region Pixel Line Renderer
    private void HandlePixelPerfectLine()
    {
        if (_lassoSpawned || _animalLassoed)
        {
            _lassoLineAnchor = _lassoInstance.transform.GetChild(0).transform.position;

            UpdatePixelPerfectLine();
        }
        else
        {
            ClearPixelPerfectLine();
        }
    }

    private void UpdatePixelPerfectLine()
    {
        Vector3 startPos = GetLineStartPosition();
        Vector3 endPos = GetLineEndPosition();
        PlotBresenhamLine(startPos, endPos);
    }

    private void ClearPixelPerfectLine()
    {
        foreach (GameObject pixel in _lassoPixelInstances)
        {
            _lassoPixelPool.ReturnPooledPixel(pixel);
        }
        _lassoPixelInstances.Clear();
    }
    private void PlotBresenhamLine(Vector3 start, Vector3 end)
    {
        int x0 = Mathf.RoundToInt(start.x * _pixelsPerUnit);
        int y0 = Mathf.RoundToInt(start.y * _pixelsPerUnit);
        int x1 = Mathf.RoundToInt(end.x * _pixelsPerUnit);
        int y1 = Mathf.RoundToInt(end.y * _pixelsPerUnit);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        int index = 0;
        while (true)
        {
            Vector3 position = new Vector3(x0 / (float)_pixelsPerUnit, y0 / (float)_pixelsPerUnit, -9f);

            if (index < _lassoPixelInstances.Count)
            {
                _lassoPixelInstances[index].transform.position = position;
                _lassoPixelInstances[index].SetActive(true);
            }
            else
            {
                GameObject pixel = _lassoPixelPool.GetPooledPixel();
                pixel.transform.position = position;
                pixel.SetActive(true);
                _lassoPixelInstances.Add(pixel);
            }


            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }

            index++;
        }

        for (int i = index; i < _lassoPixelInstances.Count; i++)
        {
            _lassoPixelInstances[i].SetActive(false);
        }
    }
    private Vector3 GetLineStartPosition()
    {
        return _animalLassoed ? _animalLassoedLineAnchor.position : _lassoThrownAnchor.position;
    }
    private Vector3 GetLineEndPosition()
    {
        return _animalLassoed ? _animalLassoAnchor : _lassoSpawned ? _lassoLineAnchor : Vector3.zero;
    }
    #endregion

    #region Set Functions
    public void SetCurrentLassoedAnimal(GameObject animal)
    {
        _currentLassoedAnimal = animal;
        _animalLassoAnchor = _currentLassoedAnimal.transform.GetChild(0).transform.position;
        _animalLassoed = true;
    }
    #endregion

}
