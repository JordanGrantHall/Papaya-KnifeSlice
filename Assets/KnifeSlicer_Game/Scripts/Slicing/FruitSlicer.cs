using System;
using System.Collections.Generic;
using UnityEngine;
namespace KnifeSlicer.Slicing
{
    public static class SpriteSliceExtensions
    {
        /// <summary>
        /// Cuts the left portion of the sprite at normalized t (0–1) and returns a new Sprite.
        /// </summary>
        public static Sprite SliceLeft(this Sprite original, float t)
        {
            // clamp to [0,1]
            t = Mathf.Clamp01(t);

            // pixel cut index
            int w = original.texture.width;
            int h = original.texture.height;
            int cutX = Mathf.RoundToInt(t * w);
            if (cutX <= 0) return null;  // nothing to slice

            // grab pixels
            var pixels = original.texture.GetPixels(0, 0, cutX, h);
            var tex = new Texture2D(cutX, h, original.texture.format, false);
            tex.SetPixels(pixels);
            tex.Apply();

            // create new sprite
            return Sprite.Create(
                tex,
                new Rect(0, 0, cutX, h),
                new Vector2(0.5f, 0.5f),
                original.pixelsPerUnit
            );
        }
    }

    public class FruitSlicer : MonoBehaviour
    {
        public Camera mainCamera;
        public SpriteRenderer targetRenderer;

        // (you can remove this if you no longer need it internally)
        private List<GameObject> _cuts = new List<GameObject>();

        /// <summary>
        /// Invoked whenever a new slice-GameObject is created.
        /// The Transform of the new slice is passed.
        /// </summary>
        public static event Action<Transform> OnObjectCut;

        private void Awake()
        {
            mainCamera = Camera.main;
            targetRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TryCut();
        }

        private void TryCut()
        {
            // world-space mouse
            Vector3 wp = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            wp.z = 0;

            // make sure we clicked inside the sprite
            if (!targetRenderer.bounds.Contains(wp))
                return;

            // normalized cut position t (0–1)
            Vector3 lp = targetRenderer.transform.InverseTransformPoint(wp);
            var b = targetRenderer.sprite.bounds;
            float t = Mathf.InverseLerp(b.min.x, b.max.x, lp.x);

            // slice off the left part
            Sprite leftSpr = targetRenderer.sprite.SliceLeft(t);
            if (leftSpr == null) return;

            // create new GO for that left fragment
            var go = new GameObject("SliceLeft");
            var rend = go.AddComponent<SpriteRenderer>();
            rend.sprite = leftSpr;
            go.transform.position = targetRenderer.transform.position;

            // keep for internal use (optional)
            _cuts.Add(go);

            // 🗲 FIRE THE EVENT so anyone listening knows about this new slice:
            OnObjectCut?.Invoke(go.transform);
        }
    }
}