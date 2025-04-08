using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelSection
    {
        public Image image;
        public AudioClip soundEffect;
        public float autoRevealDelay = 3f;
        [Range(0, 1)] public float soundVolume = 0.6f;
    }

    [System.Serializable]
    public class ComicPanel
    {
        public GameObject panelObject;
        public List<PanelSection> sections = new List<PanelSection>();
        [HideInInspector] public int currentSection = 0;
        [HideInInspector] public bool allSectionsShown = false;
    }

    [Header("Cutscene Settings")]
    [SerializeField] private List<ComicPanel> comicPanels = new List<ComicPanel>();
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float skipHoldDuration = 2f;
    [SerializeField] private string nextSceneName;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private float bgmFadeOutDuration = 1f;

    [Header("Skip Indicator")]
    [SerializeField] private GameObject skipIndicator;
    [SerializeField] private Image skipProgressCircle;
    [SerializeField] private Image leftClickIcon;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minPulseScale = 0.9f;
    [SerializeField] private float maxPulseScale = 1.1f;
    [Range(0, 1)] [SerializeField] private float inactiveAlpha = 0.2f;
    [Range(0, 1)] [SerializeField] private float activeAlpha = 1f;

    private int currentPanelIndex = 0;
    private bool isTransitioning = false;
    private float holdTime = 0f;
    private bool skipTriggered = false;
    private Vector3 originalClickIconScale;
    private Coroutine autoRevealCoroutine;
    private AudioSource sfxAudioSource;

    private void Awake()
    {
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        sfxAudioSource.playOnAwake = false;

        if (bgmAudioSource == null)
        {
            bgmAudioSource = GameObject.Find("BGM")?.GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        InitializePanels();
        InitializeSkipUI();
        ShowCurrentPanel();
    }

    private void InitializePanels()
    {
        foreach (var panel in comicPanels)
        {
            panel.panelObject.SetActive(false);
            panel.allSectionsShown = false;
            panel.currentSection = 0;
            
            foreach (var section in panel.sections)
            {
                section.image.gameObject.SetActive(false);
                SetImageAlpha(section.image, 0f);
            }
        }
    }

    private void InitializeSkipUI()
    {
        if (skipIndicator != null)
        {
            var canvasGroup = skipIndicator.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = skipIndicator.AddComponent<CanvasGroup>();
            
            canvasGroup.alpha = inactiveAlpha;
            skipProgressCircle.fillAmount = 0f;
            originalClickIconScale = leftClickIcon.transform.localScale;
        }
    }

    private void ShowCurrentPanel()
    {
        var currentPanel = comicPanels[currentPanelIndex];
        currentPanel.panelObject.SetActive(true);
        StartCoroutine(FadePanel(currentPanel.panelObject, 0f, 1f, () => {
            RevealSection(currentPanel, true);
        }));
    }

    private void Update()
    {
        HandleSkipIndicatorAnimation();

        if (skipTriggered) return;

        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            
            if (skipIndicator != null)
            {
                var canvasGroup = skipIndicator.GetComponent<CanvasGroup>();
                canvasGroup.alpha = Mathf.Lerp(inactiveAlpha, activeAlpha, Mathf.Clamp01(holdTime / 0.5f));
                
                float progress = Mathf.Clamp01(holdTime / skipHoldDuration);
                skipProgressCircle.fillAmount = progress;
            }

            if (holdTime >= skipHoldDuration)
            {
                SkipCutscene();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (holdTime < 0.2f && !isTransitioning)
            {
                var currentPanel = comicPanels[currentPanelIndex];
                if (currentPanel.currentSection < currentPanel.sections.Count)
                {
                    if (autoRevealCoroutine != null)
                    {
                        StopCoroutine(autoRevealCoroutine);
                        autoRevealCoroutine = null;
                    }
                    RevealSection(currentPanel, false);
                }
                else if (currentPanel.allSectionsShown)
                {
                    AdvancePanel();
                }
            }

            holdTime = 0f;
            if (skipIndicator != null)
            {
                var canvasGroup = skipIndicator.GetComponent<CanvasGroup>();
                canvasGroup.alpha = inactiveAlpha;
                skipProgressCircle.fillAmount = 0f;
                leftClickIcon.transform.localScale = originalClickIconScale;
            }
        }
    }

    private void RevealSection(ComicPanel panel, bool isAutoReveal)
    {
        PanelSection section = panel.sections[panel.currentSection];
        
        if (section.soundEffect != null)
        {
            sfxAudioSource.PlayOneShot(section.soundEffect, section.soundVolume);
        }

        section.image.gameObject.SetActive(true);
        StartCoroutine(FadeImage(section.image, 0f, 1f, fadeDuration));

        panel.currentSection++;
        
        if (panel.currentSection < panel.sections.Count && isAutoReveal)
        {
            autoRevealCoroutine = StartCoroutine(AutoRevealNextSection(panel, section.autoRevealDelay));
        }
        else if (panel.currentSection >= panel.sections.Count)
        {
            panel.allSectionsShown = true;
        }
    }

    private IEnumerator AutoRevealNextSection(ComicPanel panel, float delay)
    {
        yield return new WaitForSeconds(delay);
        RevealSection(panel, true);
    }

    private void AdvancePanel()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToNextPanel());
        }
    }

    private IEnumerator TransitionToNextPanel()
    {
        isTransitioning = true;

        yield return StartCoroutine(FadePanel(comicPanels[currentPanelIndex].panelObject, 1f, 0f));

        comicPanels[currentPanelIndex].panelObject.SetActive(false);

        if (currentPanelIndex < comicPanels.Count - 1)
        {
            currentPanelIndex++;
            ShowCurrentPanel();
        }
        else
        {
            yield return StartCoroutine(FadeOutBGM());
            TransitionToNextScene();
        }

        isTransitioning = false;
    }

    private IEnumerator FadeOutBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            float startVolume = bgmAudioSource.volume;
            float elapsed = 0f;

            while (elapsed < bgmFadeOutDuration)
            {
                elapsed += Time.deltaTime;
                bgmAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / bgmFadeOutDuration);
                yield return null;
            }

            bgmAudioSource.Stop();
            bgmAudioSource.volume = startVolume; // Reset volume for future use
        }
    }

    private IEnumerator FadePanel(GameObject panel, float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        CanvasGroup group = panel.GetComponent<CanvasGroup>();
        if (group == null) group = panel.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        group.alpha = startAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            yield return null;
        }

        group.alpha = endAlpha;
        onComplete?.Invoke();
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        SetImageAlpha(image, startAlpha);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetImageAlpha(image, Mathf.Lerp(startAlpha, endAlpha, elapsed / duration));
            yield return null;
        }

        SetImageAlpha(image, endAlpha);
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    private void SkipCutscene()
    {
        if (!skipTriggered)
        {
            skipTriggered = true;
            StopAllCoroutines();
            StartCoroutine(FadeOutAndTransition());
        }
    }

    private IEnumerator FadeOutAndTransition()
    {
        // Fade out all panels
        foreach (var panel in comicPanels)
        {
            if (panel.panelObject.activeSelf)
            {
                yield return StartCoroutine(FadePanel(panel.panelObject, 1f, 0f));
                panel.panelObject.SetActive(false);
            }
        }

        // Fade out BGM
        yield return StartCoroutine(FadeOutBGM());

        TransitionToNextScene();
    }

    private void TransitionToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void HandleSkipIndicatorAnimation()
    {
        if (skipIndicator != null && skipIndicator.GetComponent<CanvasGroup>().alpha > 0)
        {
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            float scale = Mathf.Lerp(minPulseScale, maxPulseScale, pulse);
            leftClickIcon.transform.localScale = originalClickIconScale * scale;
        }
    }
}
