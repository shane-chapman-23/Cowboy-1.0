using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }


    [SerializeField]
    private Transform _animalEscapedPosition;
    [SerializeField]
    private Transform _animalCaughtPosition;

    private bool _playerOnRight;

    public bool MinigameStarted { get; private set; }
    public bool GameWon { get; private set; }
    public bool GameLost { get; private set; }

    [SerializeField]
    private Image _fatigueGuage;
    [SerializeField]
    private Image _fatigueFill;

    private float _fatigueAmount = 0f;
    private float _maxFatigue = 20f;
    private float _fatigueRate = 60f;
    public float FillAmount { get; private set; }

    private float _delayTimer;
    private float _randomDelay;
    private bool _delayStarted;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleFatigueGuage();
        HandleGameLoss();
        HandleGameWin();
    }

    #region Fatigue Functions
    private void HandleFatigueGuage()
    {
        bool startFatigueConditions = MinigameStarted && !GameWon && !GameLost;

        if (startFatigueConditions)
        {
            _fatigueGuage.gameObject.SetActive(true);

            float input = Player.Instance.InputHandler.NormInputX;
            float directionFactor = _playerOnRight ? -1 : 1;

            if (input != 0)
            {
                if (!_delayStarted)
                {
                    _delayStarted = true;
                    _delayTimer = 0;
                    _randomDelay = UnityEngine.Random.Range(0.5f, 1.5f);
                }

                _delayTimer += Time.deltaTime;

                if (_delayTimer >= _randomDelay)
                {
                    UpdateFatigueGuage(input, directionFactor);
                }
            }
            else
            {
                _delayStarted = false;
                UpdateFatigueGuage(input, directionFactor);
            }
        }
        else
        {
            _fatigueGuage.gameObject.SetActive(false);
        }
    }

    private void UpdateFatigueGuage(float input, float directionFactor)
    {
        if (input * directionFactor < 0)
        {
            _fatigueAmount += _fatigueRate * Time.deltaTime;
        }
        else
        {
            _fatigueAmount -= _fatigueRate * Time.deltaTime * 20f;
        }

        _fatigueAmount = Mathf.Clamp(_fatigueAmount, 0, _maxFatigue);
        FillAmount = _fatigueAmount / _maxFatigue;
        _fatigueFill.rectTransform.localScale = new Vector3(1, FillAmount, 1);

        if (FillAmount == 1)
        {
            GameLoss();
        }
    }
    #endregion

    public void SetMinigameStartedTrue()
    {
        MinigameStarted = true;
    }

    public void SetMiniGameStartedFalse()
    {
        MinigameStarted = false;
    }

    public void SetAnimalPositions(Vector2 animalPosition)
    {        
        if (_playerOnRight)
        {
            _animalEscapedPosition.position = new Vector2((animalPosition.x - 2f), animalPosition.y);
        }
        else
        {
            _animalEscapedPosition.position = new Vector2((animalPosition.x + 2f), animalPosition.y);
        }
    }

    public void SetPlayerOnRightTrue()
    {
        _playerOnRight = true;
    }

    public void SetPlayerOnRightFalse()
    {
        _playerOnRight = false;
    }

    private void HandleGameWin()
    {

        if (MinigameStarted)
        {
            Vector2 animalPosition = LassoController.Instance.CurrentLassoedAnimal.transform.position;

            bool winConditionMet = _playerOnRight
                ? animalPosition.x >= _animalCaughtPosition.position.x
                : animalPosition.x <= _animalCaughtPosition.position.x;

            if (winConditionMet)
            {
                GameWin();
                SetMiniGameStartedFalse();
            }
        }
    }

    private void HandleGameLoss()
    {

        if (MinigameStarted)
        {
            Vector2 animalPosition = LassoController.Instance.CurrentLassoedAnimal.transform.position;

            bool lossConditionMet = _playerOnRight
                ? animalPosition.x <= _animalEscapedPosition.position.x
                : animalPosition.x >= _animalEscapedPosition.position.x;

            if (lossConditionMet)
            {
                GameLoss();
                SetMiniGameStartedFalse();
            }
        }
    }
    private void GameWin()
    {
        GameWon = true;
    }

    public void SetGameWonToFalse()
    {
        GameWon = false;
    }

    private void GameLoss()
    {
        GameLost = true;
        StartCoroutine(ResetMinigame());
    }

    private IEnumerator ResetMinigame()
    {
        yield return new WaitForSeconds(0.2f);
        GameLost = false;
        GameWon = false;
    }
}
