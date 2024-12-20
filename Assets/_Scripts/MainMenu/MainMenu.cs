using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public Image fadeImage;
    private float fadeDuration = 1.0f;
    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator StartGame()
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeTransition(true));
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainGame");
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
}
