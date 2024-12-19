using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Transform _fadeToBlackTrigger;
    [SerializeField]
    private Transform _endGame;

    public Transform mapEdgeLeft;
    public Transform mapEdgeRight;
    public Transform endGameSpawn;

    public bool endGame;

    private float fadeDuration = 1.0f;
    private bool _isTransitioning;

    public int animalsCaught;

    public Image fadeImage;
    public Image Title;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        //animalsCaught = 5;
    }
    private void Update()
    {
        StartAnimalCaughtTransition();
        DisablePlayerInputOnEndGame();
    }

    #region
    private void DisablePlayerInputOnEndGame()
    {
        if (Player.Instance.transform.position.x >= _endGame.position.x && animalsCaught == 5)
        {
            Player.Instance.DisableInput();
            endGame = true;
            StartCoroutine(ShowTitleScreen());
        }
    }

    private IEnumerator ShowTitleScreen()
    {
        yield return new WaitForSeconds(3);

        Title.gameObject.SetActive(true);
    }
    #endregion

    #region Animal Caught Transition Functions
    private void StartAnimalCaughtTransition()
    {
        bool transitionConditionsMet = Player.Instance.StateMachineController.CurrentState == Player.Instance.CaughtAnimalState
            && Player.Instance.transform.position.x < _fadeToBlackTrigger.position.x;

        if (transitionConditionsMet && !_isTransitioning)
        {
            StartCoroutine(AnimalCaughtTransition());
        }
    }

    private IEnumerator AnimalCaughtTransition()
    {
        _isTransitioning = true;

        yield return StartCoroutine(FadeToBlack());

        MinigameManager.Instance.SetGameWonToFalse();
        LassoController.Instance.DestroyCurrentAnimal();
        LassoController.Instance.SetCurrentLassoedAnimalNull();
        Player.Instance.ChangeState(Player.Instance.IdleState);
        Player.Instance.transform.position = Player.Instance.playerPositionOnAnimalLassoed;

        HandlePlayerFacingDirection();

        AnimalManager.Instance.SpawnAnimal();

        animalsCaught++;

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeFromBlack());

        _isTransitioning = false;
    }

    private void HandlePlayerFacingDirection()
    {
        if (Player.Instance.facingDirection == -1)
        {
            Player.Instance.Flip();
        }
    }

    private IEnumerator FadeTransition(bool fadeToBlack)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        color.a = fadeToBlack ? 0f : 1f;
        fadeImage.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = fadeToBlack
                ? Mathf.Clamp01(elapsed / fadeDuration)
                : 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = fadeToBlack ? 1f : 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeToBlack()
    {
        yield return StartCoroutine(FadeTransition(true));
    }

    private IEnumerator FadeFromBlack()
    {
        yield return StartCoroutine(FadeTransition(false));
    }

    #endregion
}
