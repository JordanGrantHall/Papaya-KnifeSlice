using UnityEngine;
namespace KnifeSlicer.Example
{

    public class RestartBehaviour : MonoBehaviour
    {

        /// <summary>
        /// Simple restart method triggered by UI.Button.
        /// </summary>
        public void RestartScene()
        {
            Application.LoadLevel(0);
        }

    }
}