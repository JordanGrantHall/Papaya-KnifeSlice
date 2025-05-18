using DG.Tweening;
using System.Collections;
using UnityEngine;

public class KnifeCutter : MonoBehaviour
{
    [SerializeField]
    private VerticalUIRawImageCutter _cutter; // assign the UI element that has your cutter component

    private RectTransform _knifeRect;
    [SerializeField]
    private RectTransform _rotateRect;
    private Canvas _canvas;
    private AudioSource _sliceAudioSource;
    void Awake()
    {
        // cache our own RectTransform (knife is a UI element)
        _knifeRect = GetComponent<RectTransform>();
        _sliceAudioSource = GetComponent<AudioSource>();
        // find the Canvas up the hierarchy
        _canvas = GetComponentInParent<Canvas>();
        if (_cutter == null)
            Debug.LogWarning("KnifeCutter: no VerticalUIRawImageCutter assigned!");

        this.enabled = false;
    }

    void OnEnable()
    {
        StartCoroutine(InputLoop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator InputLoop()
    {
        // Coroutine instead of Update()
        while (true)
        {
            if (Input.GetMouseButtonDown(0) && _cutter != null)
            {
                // Convert our UI‐space RectTransform position into screen‐space
                // If your Canvas is Screen Space - Overlay, just pass null for cam.
                Camera cam = (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                             ? null
                             : _canvas.worldCamera;

                Vector2 knifeScreenPos = RectTransformUtility.WorldToScreenPoint(
                    cam,
                    _knifeRect.position
                );


                if (_cutter.TrySliceAtScreenPosition(knifeScreenPos))
                {
                    CutTween();
                }
            }
            yield return null;
        }
    }

    Tween _cutTween;

    public void CutTween()
    {
        // 1. Kill any existing tween
        if (_cutTween != null)
        {
            _cutTween.Kill();
            _cutTween = null;
        }

        // 2. Create a new tween that rotates forward then back
        _cutTween = _rotateRect
            .DOLocalRotate(new Vector3(0, 40, 130), 0.1f)
            .From(new Vector3(0, 40, 0))
            .SetEase(Ease.Linear)
            // Loop twice: once forward, once backward (yoyo)
            .SetLoops(2, LoopType.Yoyo);

        AudioClip clip = CustomisationContainer.Instance.GetKnifeSliceSoundFX();
        if (clip != null)
            _sliceAudioSource.PlayOneShot(clip, 0.25f);
    }


}