using System.Collections.Generic;
using System;
using UnityEngine;

namespace KnifeSlicer.UnitySpriteCutter.Cutters
{
    /// <summary>
    /// Always cuts a vertical line at the clicked X and broadcasts the new slice.
    /// </summary>
    public class VerticalLineCutterBehaviour : MonoBehaviour
    {
        [Tooltip("Which layers to consider when cutting")]
        public LayerMask layerMask;

        // store your click’s screen position so X stays consistent
        private Vector2 _mouseScreenPos;

        /// <summary>
        /// Fired whenever a new slice GameObject is created.
        /// Subscribers receive the Transform of the new slice.
        /// </summary>
        public static event Action<Transform> OnObjectCut;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseScreenPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                PerformVerticalCut(_mouseScreenPos, layerMask.value);
            }
        }

        private void PerformVerticalCut(Vector2 screenPos, int mask)
        {
            Camera cam = Camera.main;
            float camZ = -cam.transform.position.z;

            Vector3 bottomScreen = new Vector3(screenPos.x, 0f, camZ);
            Vector3 topScreen = new Vector3(screenPos.x, Screen.height, camZ);

            Vector2 lineStart = cam.ScreenToWorldPoint(bottomScreen);
            Vector2 lineEnd = cam.ScreenToWorldPoint(topScreen);

            var toCut = new List<GameObject>();
            foreach (var hit in Physics2D.LinecastAll(lineStart, lineEnd, mask))
            {
                if (hit.transform.GetComponent<SpriteRenderer>() != null ||
                    hit.transform.GetComponent<MeshRenderer>() != null)
                {
                    toCut.Add(hit.transform.gameObject);
                }
            }

            foreach (var go in toCut)
            {
                var input = new SpriteCutterInput
                {
                    lineStart = lineStart,
                    lineEnd = lineEnd,
                    gameObject = go,
                    gameObjectCreationMode = SpriteCutterInput.GameObjectCreationMode.CUT_OFF_COPY
                };
                SpriteCutterOutput output = SpriteCutter.Cut(input);
                if (output != null && output.secondSideGameObject != null)
                {
                    // Broadcast the new (left) slice
                    OnObjectCut?.Invoke(output.firstSideGameObject.transform);
                }
            }
        }
    }
}
