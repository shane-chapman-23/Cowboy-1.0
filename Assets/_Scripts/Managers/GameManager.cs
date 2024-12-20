using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Transform _fadeToBlackTrigger;
    [SerializeField]
    private Transform _endGame;
    [SerializeField]
    private GameObject _menu;

    private bool _isMenuActive = false;

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

        SetInitialBlackScreen();
    }

    private void Start()
    {
        animalsCaught = 5;

        StartCoroutine(StartGame());
        HandlePlayerRespawn();
    }
    private void Update()
    {
        StartAnimalCaughtTransition();
        HandleEndGame();
        HandleInGameMenu();
    }

    #region
    private void HandleEndGame()
    {
        if (Player.Instance.transform.position.x >= _endGame.position.x && animalsCaught == 5 && !endGame)
        {
            endGame = true;
            Player.Instance.DisableInput();
            StartCoroutine(ShowTitleScreen());
            StartCoroutine(ReturnToMainMenu());
        }
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5);
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
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

    private void SetInitialBlackScreen()
    {
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator StartGame()
    {


        yield return new WaitForSeconds(2f);

        StartCoroutine(FadeFromBlack());
    }

    private void HandleInGameMenu()
    {
        if (Player.Instance.InputHandler.EscapePressed)
        {
            ToggleMenu();
            Player.Instance.InputHandler.SetEscapeFalse();
        }
    }

    private void ToggleMenu()
    {
        _isMenuActive = !_isMenuActive;
        _menu.SetActive(_isMenuActive);
    }



    private IEnumerator AnimalCaughtTransition()
    {
        _isTransitioning = true;

        yield return StartCoroutine(FadeToBlack());

        MinigameManager.Instance.SetGameWonToFalse();
        LassoController.Instance.DestroyCurrentAnimal();
        LassoController.Instance.SetCurrentLassoedAnimalNull();
        Player.Instance.ChangeState(Player.Instance.IdleState);

        animalsCaught++;

        HandlePlayerRespawn();
        HandlePlayerFacingDirection();

        AnimalManager.Instance.SpawnAnimal();

        

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeFromBlack());

        _isTransitioning = false;
    }

    private void HandlePlayerRespawn()
    {
        if (animalsCaught > 4)
        {
            Player.Instance.transform.position = new Vector2(endGameSpawn.position.x, Player.Instance.transform.position.y);
        }
        else
        {
            Player.Instance.transform.position = Player.Instance.playerPositionOnAnimalLassoed;
        }
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
