using KnifeSlicer.Core;
using KnifeSlicer.Core.Events;
using KnifeSlicer.Customisation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToCutListener : BaseBehaviour { 
    RawImage _spriteRend;
    RawImagePreserveAspect _rawImagePreserveAspect;

    private void Awake()
    {
        _spriteRend = GetComponentInChildren<RawImage>();
        _rawImagePreserveAspect = GetComponentInChildren<RawImagePreserveAspect>();
    }

    [Topic(KnifeSlicerCustomisationIds.ON_OBjECT_TO_CUT_SELECTED)]
    public void OnKnifeSelected(object sender, Texture2D texture)
    {
        Texture _objectToCut = texture;

        _spriteRend.texture = _objectToCut;
        _rawImagePreserveAspect.UpdateSize();
    }
}
