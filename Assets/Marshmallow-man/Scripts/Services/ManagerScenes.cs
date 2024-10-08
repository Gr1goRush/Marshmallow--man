using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ManagerScenes : MonoBehaviour
{
    public static ManagerScenes Instance { get; private set; }
    public string NameActiveScene => SceneManager.GetActiveScene().name;
    [SerializeField] private LoadingView _loadingView;
    [HideInInspector]
    public UnityEvent LoadingSceneEventHandler = new UnityEvent();
    [HideInInspector]
    public UnityEvent StartLoadingSceneEventHandler = new UnityEvent();

    public void LoadAsyncFromCoroutine(string nameScene) => StartCoroutine(LoadAsync(nameScene));

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
        DontDestroyOnLoad(this);
        StartLoadingSceneEventHandler.AddListener(() => { _loadingView.OnStarLoadingScene(); });
        LoadingSceneEventHandler.AddListener(() => { StartCoroutine(_loadingView.StartLoading()); });
    }

    private IEnumerator LoadAsync(string nameScene)
    {
        var operation = SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);
        StartLoadingSceneEventHandler?.Invoke();

        while (operation.progress <= 1)
        {
            var progressInPercent = (int)(operation.progress * 100);
            LoadingSceneEventHandler?.Invoke();

            yield return null;
        }
    }
}
