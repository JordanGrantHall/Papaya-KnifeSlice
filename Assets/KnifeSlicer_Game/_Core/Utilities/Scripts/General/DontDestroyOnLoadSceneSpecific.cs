using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeSlicer.General.Utility
{
    public class DontDestroyOnLoadSceneSpecific : MonoBehaviour
    {
        [SerializeField]
        private string _sceneNamePrefix = "KnifeSlicer_";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            TryDestroyIfOutsidePrefix(newScene.name);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
                TryDestroyIfOutsidePrefix(scene.name);
        }

        private void TryDestroyIfOutsidePrefix(string sceneName)
        {
            if (!sceneName.StartsWith(_sceneNamePrefix))
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
