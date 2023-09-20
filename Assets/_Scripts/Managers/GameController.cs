using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fade;
    private bool sceneLoaded = false;

    private void Start()
    {
        if (!_fade)
            return;

        _fade.gameObject.SetActive(true);
        _fade.blocksRaycasts = true;
        _fade.alpha = 1;

        LeanTween.alphaCanvas(_fade, 0f, 1f).setOnComplete(() => {
            _fade.blocksRaycasts = false;
        });
    }
    private void LoadScene(string sceneName)
    {
        if (_fade == null)
            return;

        _fade.gameObject.SetActive(true);
        _fade.blocksRaycasts = true;
        _fade.alpha = 0;

        LeanTween.alphaCanvas(_fade, 1f, 1f).setOnComplete(() => {
            _fade.blocksRaycasts = false;
            StartCoroutine(LoadAsync(sceneName));
        });
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if (progress >= 1f && !sceneLoaded)
            {
                sceneLoaded = true;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneName(string sceneName)
    {
        LoadScene(sceneName);
    }
}