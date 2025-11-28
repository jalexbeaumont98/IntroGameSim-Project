using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private float defaultFadeDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private bool isFading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        // Start fully black (alpha 1), then fade in on first scene
        canvasGroup.alpha = 1f;
        StartCoroutine(Fade(1f, 0f, defaultFadeDuration));
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeOutLoad(sceneName, defaultFadeDuration));
    }

    public void FadeIn(float duration = -1f)
    {
        if (!isFading)
            StartCoroutine(Fade(1f, 0f, duration < 0 ? defaultFadeDuration : duration));
    }

    public void FadeOut(float duration = -1f)
    {
        if (!isFading)
            StartCoroutine(Fade(0f, 1f, duration < 0 ? defaultFadeDuration : duration));
    }

    private IEnumerator FadeOutLoad(string sceneName, float duration)
    {
        // Fade to black
        yield return Fade(0f, 1f, duration);

        // Load the new scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade back in
        yield return Fade(1f, 0f, duration);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        isFading = true;

        float t = 0f;
        canvasGroup.alpha = from;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // not affected by Time.timeScale
            float lerp = Mathf.Clamp01(t / duration);
            canvasGroup.alpha = Mathf.Lerp(from, to, lerp);
            yield return null;
        }

        canvasGroup.alpha = to;
        isFading = false;
    }
}
