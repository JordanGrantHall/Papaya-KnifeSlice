using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImagePreserveAspect : MonoBehaviour
{
    [Tooltip("If left blank, will use this RawImage's parent RectTransform.")]
    [SerializeField] private RectTransform _parentRect;

    private RawImage _rawImage;
    private RectTransform _rt;

    private void Awake()
    {
        FindReferences();
    }

    private void OnEnable() => UpdateSize();
    private void OnValidate() => UpdateSize();  // for editor tweaks

    // Called when this RectTransform’s own size changes (e.g. if your script or layout group resizes it).
    private void OnRectTransformDimensionsChange()
    {
        // Only update if we’re active
        if (isActiveAndEnabled) UpdateSize();
    }

    public void FindReferences()
    {
        _rawImage = GetComponent<RawImage>();
        _rt = _rawImage.rectTransform;

        if (_parentRect == null && _rt.parent is RectTransform parent)
            _parentRect = parent;
    }

    /// <summary>
    /// Resizes this RawImage to be as large as possible inside _parentRect
    /// without exceeding its width or height, preserving the texture’s aspect.
    /// </summary>
    [ContextMenu("Update Size")]
    public void UpdateSize()
    {
        FindReferences();

        if (_rawImage.texture == null || _parentRect == null)
            return;

        float pw = _parentRect.rect.width;
        float ph = _parentRect.rect.height;

        if (pw <= 0 || ph <= 0)
            return;

        float tw = _rawImage.texture.width;
        float th = _rawImage.texture.height;
        if (tw <= 0 || th <= 0)
            return;

        float imageAspect = tw / th;
        float parentAspect = pw / ph;

        float finalW, finalH;
        if (parentAspect > imageAspect)
        {
            // Parent is “wider” than image aspect → fit by height
            finalH = ph;
            finalW = ph * imageAspect;
        }
        else
        {
            // Parent is “taller” (or equal) → fit by width
            finalW = pw;
            finalH = pw / imageAspect;
        }

        _rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalW);
        _rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalH);
    }
}
