using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f; // Default fade duration, editable in the Inspector

    private static FadeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static FadeManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("FadeManager instance is missing!");
            }
            return instance;
        }
    }

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }

    public void FadeIn()
    {
        fadeCanvasGroup.DOFade(1, fadeDuration);
    }

    public void FadeOut()
    {
        fadeCanvasGroup.DOFade(0, fadeDuration);
    }

    public IEnumerator FadeInOut(float inDuration, float outDuration, System.Action onFadeOutComplete = null)
    {
        fadeCanvasGroup.DOFade(1, inDuration).OnComplete(() =>
        {
            onFadeOutComplete?.Invoke();
            fadeCanvasGroup.DOFade(0, outDuration);
        });
        yield return new WaitForSeconds(inDuration + outDuration);
    }
}
