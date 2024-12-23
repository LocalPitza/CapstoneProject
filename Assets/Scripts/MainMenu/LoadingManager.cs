using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Slider LoadingBarFill;

    public void LoadScene(string sceneID, Action onFirstFrameLoad = null)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAsync(sceneID, onFirstFrameLoad));
    }

    IEnumerator LoadSceneAsync(string sceneID, Action onFirstFrameLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        DontDestroyOnLoad(gameObject);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.value = progressValue;

            yield return null;

            Debug.Log("Loading");
        }

        Debug.Log("Scene Loaded");

        yield return new WaitForEndOfFrame();

        Debug.Log("First Frame Loaded");

        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
    }
}
