using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SRAspectSizeController  : MonoBehaviour
{
    [SerializeField]
    private Vector2 _targetSize = Vector2.one;

    private SpriteRenderer _sr;
    private Sprite _lastSprite;

    private void OnEnable()
    {
        _sr = GetComponent<SpriteRenderer>();
        _lastSprite = _sr.sprite;
        UpdateSize();
    }

    private void LateUpdate()
    {
        // detect sprite swaps at runtime
        if (_sr.sprite != _lastSprite)
        {
            _lastSprite = _sr.sprite;
            UpdateSize();
        }
    }

    private void OnValidate()
    {
        UpdateSize();
    }

    /// <summary>
    /// Call this from code to change the target size and immediately apply it.
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
        if (_sr == null) _sr = GetComponent<SpriteRenderer>();
        var sprite = _sr.sprite;
        if (sprite == null) return;

        // Convert sprite pixels to world units
        Vector2 spriteWorldSize = new Vector2(
            sprite.rect.width / sprite.pixelsPerUnit,
            sprite.rect.height / sprite.pixelsPerUnit
        );

        // Compute needed scale so that the sprite covers _targetSize units
        Vector3 newScale = new Vector3(
            _targetSize.x / spriteWorldSize.x,
            _targetSize.y / spriteWorldSize.y,
            1f
        );

        transform.localScale = newScale;
    }
}
