using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Make sure fade image starts transparent
            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = 0f;
                fadeImage.color = color;
                fadeImage.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeToSceneWithDelay(string sceneName, float delay = 3f)
    {
        StartCoroutine(FadeToSceneCoroutine(sceneName, delay));
    }

    private IEnumerator FadeToSceneCoroutine(string sceneName, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Activate fade image
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
        }

        // Fade to white
        yield return StartCoroutine(FadeToWhite());

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // Fade from white (this will happen in the new scene)
        yield return StartCoroutine(FadeFromWhite());
    }

    private IEnumerator FadeToWhite()
    {
        if (fadeImage == null) yield break;

        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Ensure it's fully opaque
        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator FadeFromWhite()
    {
        if (fadeImage == null) yield break;

        // Small delay before fading out in new scene
        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }

        // Ensure it's fully transparent and deactivate
        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }

    // Public method for immediate scene transition with fade (no delay)
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeToSceneCoroutine(sceneName, 0f));
    }
}