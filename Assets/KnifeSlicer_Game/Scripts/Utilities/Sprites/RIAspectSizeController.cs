using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(RawImage))]
public class RIAspectSizeController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _targetSize = new Vector2(100, 100);  // Size in pixels

    private RawImage _ri;
    private Texture _lastTexture;
    private RectTransform _rect;

    private void OnEnable()
    {
        _ri = GetComponent<RawImage>();
        _rect = GetComponent<RectTransform>();
        _lastTexture = _ri.texture;
        UpdateSize();
    }

    private void LateUpdate()
    {
        // detect texture swaps at runtime
        if (_ri.texture != _lastTexture)
        {
            _lastTexture = _ri.texture;
            UpdateSize();
        }
    }

    private void OnValidate()
    {
        // make sure we have our refs in the editor
        _ri = GetComponent<RawImage>();
        _rect = GetComponent<RectTransform>();
        UpdateSize();
    }

    /// <summary>
    /// Call this from code to change the target size (in pixels) and immediately apply it.
    /// </summary>
    public void SetTargetSize(Vector2 size)
    {
        _targetSize = size;
        UpdateSize();
    }

    /// <summary>
    /// Force-refresh in the Editor via right-click menu.
    /// </summary>
    [ContextMenu("Refresh Size")]
    private void UpdateSize()
    {
        if (_ri == null) _ri = GetComponent<RawImage>();
        if (_rect == null) _rect = GetComponent<RectTransform>();

        // If you’d rather scale relative to the texture’s native resolution:
        if (_ri.texture != null)
        {
            Vector2 texSize = new Vector2(_ri.texture.width, _ri.texture.height);
            Vector2 scale = new Vector2(
                _targetSize.x / texSize.x,
                _targetSize.y / texSize.y
            );
            _rect.localScale = new Vector3(scale.x, scale.y, 1f);
        }
        else
        {
            // Fallback: just set the rect size directly
            _rect.sizeDelta = _targetSize;
        }
    }
}
