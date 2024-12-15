using System.Collections;
using UnityEngine;

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

    public float FillAmount { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleGameLoss();
    }

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
