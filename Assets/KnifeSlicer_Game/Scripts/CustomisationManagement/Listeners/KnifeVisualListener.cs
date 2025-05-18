using KnifeSlicer.Core;
using KnifeSlicer.Core.Events;
using KnifeSlicer.Customisation;
using UnityEngine;
using UnityEngine.UI;

public class KnifeVisualListener : BaseBehaviour
{
    RawImage _spriteRend;
    RawImagePreserveAspect _rawImagePreserveAspect;

    private void Awake()
    {
        _spriteRend = GetComponentInChildren<RawImage>();
        _rawImagePreserveAspect = GetComponentInChildren<RawImagePreserveAspect>();
    }

    [Topic(KnifeSlicerCustomisationIds.ON_KNIFE_CUSTOMISATION_SELECTED)]
    public void OnKnifeSelected(object sender, Texture2D texture)
    {
        Texture _knifeSelectedSprite = texture;
        if (!_spriteRend)
            _spriteRend = GetComponent<RawImage>();

        _spriteRend.texture = _knifeSelectedSprite;
        _rawImagePreserveAspect.UpdateSize();
    }
}
