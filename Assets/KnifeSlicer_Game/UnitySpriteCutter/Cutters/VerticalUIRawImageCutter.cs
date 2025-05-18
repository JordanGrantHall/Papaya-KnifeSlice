using KnifeSlicer.Core;
using KnifeSlicer.GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerticalUIRawImageCutter : BaseBehaviour
{
    Vector2 _downPos;
    GraphicRaycaster _raycaster;
    Canvas _canvas;

    // NOTE: swap this out for your Topic System call if you have one
    public static event Action<Transform> OnRawImageSliced;
    //                             (leftPiece, rightPiece)

    void Awake()
    {
        _raycaster = GetComponentInParent<GraphicRaycaster>();
        _canvas = GetComponentInParent<Canvas>();
    }

    /*public void OnPointerDown(PointerEventData e)
    {
        _downPos = e.position;
    }

    public void OnPointerUp(PointerEventData e)
    {
        var pd = new PointerEventData(EventSystem.current) { position = _downPos };
        var results = new List<RaycastResult>();
        _raycaster.Raycast(pd, results);

        foreach (var r in results)
        {
            var raw = r.gameObject.GetComponent<RawImage>();
            if (raw != null) SliceRawImage(raw, _downPos.x);
        }
    }*/



    public bool TrySliceAtScreenPosition(Vector2 screenPosition)
    {
        bool wasCutSuccessful = false;

        // build a fake PointerEventData
        var pd = new PointerEventData(EventSystem.current) { position = screenPosition };
        var results = new List<RaycastResult>();
        _raycaster.Raycast(pd, results);

        // try to slice every RawImage under that point
        foreach (var r in results)
        {
            if(!r.gameObject.GetComponent<VerticalUIRawImageCutter>())
                continue;

            var raw = r.gameObject.GetComponent<RawImage>();
            if (raw != null)
            {
                SliceRawImage(raw, screenPosition.x);
                wasCutSuccessful = true;
            }
        }

        return wasCutSuccessful;
    }

    public void SliceRawImage(RawImage original, float screenCutX)
    {
        var rt = original.rectTransform;
        var size = rt.rect.size;
        var pos = rt.anchoredPosition;
        var pv = rt.pivot;
        var uv = original.uvRect;

        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
          rt, new Vector2(screenCutX, 0f), _canvas.worldCamera, out local
        );
        float leftEdge = -pv.x * size.x;
        float nx = Mathf.Clamp01((local.x - leftEdge) / size.x);

        float wL = size.x * nx;
        float wR = size.x - wL;

        Rect uvL = new Rect(uv.x, uv.y, uv.width * nx, uv.height);
        Rect uvR = new Rect(uv.x + uv.width * nx, uv.y, uv.width * (1 - nx), uv.height);

        var right = Instantiate(original, rt.parent);
        CopyRect(rt, right.rectTransform);
        right.rectTransform.sizeDelta = new Vector2(wR, size.y);
        right.rectTransform.anchoredPosition = pos
          + Vector2.right * (wL * (1 - pv.x));
        right.uvRect = uvR;

        original.rectTransform.sizeDelta = new Vector2(wL, size.y);
        original.rectTransform.anchoredPosition = pos
          + Vector2.right * (pv.x * (wL - size.x));
        original.uvRect = uvL;

        Publish(GameEventIds.ON_KNIFE_CUT, original.transform);
    }

    void CopyRect(RectTransform src, RectTransform dst)
    {
        dst.anchorMin = src.anchorMin;
        dst.anchorMax = src.anchorMax;
        dst.pivot = src.pivot;
        dst.localScale = src.localScale;
        dst.localRotation = src.localRotation;
    }
}
