using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class ComicPanel
    {
        public GameObject panelObject;
        public List<Image> sections = new List<Image>();
        [HideInInspector] public int currentSection = 0;
        [HideInInspector] public bool allSectionsShown = false;
    }

    [Header("Cutscene Settings")]
    [SerializeField] private List<ComicPanel> comicPanels = new List<ComicPanel>();
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float skipHoldDuration = 2f;
    [SerializeField] private string nextSceneName;

    [Header("Skip Indicator")]
    [SerializeField] private GameObject skipIndicator;
    [SerializeField] private Image skipProgressCircle;
    [SerializeField] private Image leftClickIcon;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minPulseScale = 0.9f;
    [SerializeField] private float maxPulseScale = 1.1f;

    private int currentPanelIndex = 0;
    private bool isTransitioning = false;
    private float holdTime = 0f;
    private bool skipTriggered = false;
    private Vector3 originalClickIconScale;
    private bool waitingForClick = false;

    private void Start()
    {
        InitializePanels();
        InitializeSkipUI();
        ShowCurrentPanel();
    }

    private void InitializePanels()
    {
        // Hide all panels and sections initially
        foreach (var panel in comicPanels)
        {
            panel.panelObject.SetActive(false);
            panel.allSectionsShown = false;
            panel.currentSection = 0;
            
            foreach (var section in panel.sections)
            {
                section.gameObject.SetActive(false);
                SetImageAlpha(section, 0f);
            }
        }
    }

    private void InitializeSkipUI()
    {
        if (skipIndicator != null)
        {
            skipIndicator.SetActive(false);
            skipProgressCircle.fillAmount = 0f;
            originalClickIconScale = leftClickIcon.transform.localScale;
        }
    }

    private void ShowCurrentPanel()
    {
        var currentPanel = comicPanels[currentPanelIndex];
        currentPanel.panelObject.SetActive(true);
        StartCoroutine(FadePanel(currentPanel.panelObject, 0f, 1f));
        waitingForClick = true;
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
                if (!skipIndicator.activeSelf)
                {
                    skipIndicator.SetActive(true);
                    skipProgressCircle.fillAmount = 0f;
                }
                
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
            if (holdTime < 0.2f && waitingForClick && !isTransitioning)
            {
                RevealNextSectionOrPanel();
            }

            holdTime = 0f;
            if (skipIndicator != null)
            {
                skipIndicator.SetActive(false);
                leftClickIcon.transform.localScale = originalClickIconScale;
            }
        }
    }

    private void RevealNextSectionOrPanel()
    {
        var currentPanel = comicPanels[currentPanelIndex];

        // If not all sections shown, reveal next one
        if (currentPanel.currentSection < currentPanel.sections.Count)
        {
            StartCoroutine(RevealSection(currentPanel));
        }
        // All sections shown, go to next panel
        else if (!isTransitioning)
        {
            currentPanel.allSectionsShown = true;
            waitingForClick = false;
            StartCoroutine(TransitionToNextPanel());
        }
    }

    private System.Collections.IEnumerator RevealSection(ComicPanel panel)
    {
        waitingForClick = false;
        
        // Get current section and reveal it
        Image section = panel.sections[panel.currentSection];
        section.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(section, 0f, 1f, fadeDuration));

        panel.currentSection++;
        waitingForClick = true; // Ready for next click
    }

    private System.Collections.IEnumerator TransitionToNextPanel()
    {
        isTransitioning = true;

        // Fade out current panel
        yield return StartCoroutine(FadePanel(comicPanels[currentPanelIndex].panelObject, 1f, 0f));

        // Hide current panel
        comicPanels[currentPanelIndex].panelObject.SetActive(false);

        // Move to next panel if available
        if (currentPanelIndex < comicPanels.Count - 1)
        {
            currentPanelIndex++;
            ShowCurrentPanel();
        }
        else
        {
            TransitionToNextScene();
        }

        isTransitioning = false;
    }

    private System.Collections.IEnumerator FadePanel(GameObject panel, float startAlpha, float endAlpha)
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
    }

    private System.Collections.IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
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
            StartCoroutine(FadeOutAllPanelsAndTransition());
        }
    }

    private System.Collections.IEnumerator FadeOutAllPanelsAndTransition()
    {
        foreach (var panel in comicPanels)
        {
            if (panel.panelObject.activeSelf)
            {
                yield return StartCoroutine(FadePanel(panel.panelObject, 1f, 0f));
                panel.panelObject.SetActive(false);
            }
        }
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
        if (skipIndicator != null && skipIndicator.activeSelf)
        {
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            float scale = Mathf.Lerp(minPulseScale, maxPulseScale, pulse);
            leftClickIcon.transform.localScale = originalClickIconScale * scale;
        }
    }
}
