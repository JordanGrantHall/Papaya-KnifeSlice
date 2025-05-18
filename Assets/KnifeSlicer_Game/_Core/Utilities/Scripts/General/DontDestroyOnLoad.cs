using UnityEngine;
using UnityEngine.SceneManagement;

namespace KnifeSlicer.General.Utility
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}