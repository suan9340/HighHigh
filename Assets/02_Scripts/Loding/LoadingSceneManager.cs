using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingSceneManager : MonoBehaviour
{
    #region SingleTon

    private static LoadingSceneManager _instance = null;
    public static LoadingSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var _obj = FindObjectOfType<LoadingSceneManager>();
                if (_obj == null)
                {
                    _instance = _obj;
                }
                else
                {
                    _instance = Create();
                }
            }
            return _instance;
        }
    }

    private static LoadingSceneManager Create()
    {
        return Instantiate(Resources.Load<LoadingSceneManager>("LoadingUI/Canvas (Loading)"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public CanvasGroup canvasGroup;
    public Image progressBar;

    private string loadSceneName;

    public void LoadScene(string _sceneName)
    {
        gameObject.SetActive(true);

        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = _sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0;
        yield return StartCoroutine(Fade(true));

        AsyncOperation _op = SceneManager.LoadSceneAsync(loadSceneName);
        _op.allowSceneActivation = false;

        var _timer = 0f;
        while (!_op.isDone)
        {
            yield return null;

            if (_op.progress < 0.9f)
            {
                progressBar.fillAmount = _op.progress;
            }
            else
            {
                _timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, _timer);
                if (progressBar.fillAmount >= 1f)
                {
                    _op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene _arg0, LoadSceneMode _ar1)
    {
        if (_arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private IEnumerator Fade(bool _isFadeIn)
    {
        var _timer = 0f;

        while (_timer <= 1f)
        {
            yield return null;
            _timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = _isFadeIn ? Mathf.Lerp(0f, 1f, _timer) : Mathf.Lerp(1f, 0f, _timer);
        }

        if (!_isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}
