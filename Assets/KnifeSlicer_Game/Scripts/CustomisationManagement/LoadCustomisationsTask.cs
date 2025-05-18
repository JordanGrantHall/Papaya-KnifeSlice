using KnifeSlicer.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadCustomisationsTask : TaskBase
{
    public RawImage _knifeImage;
    public RawImage _objectToCutImage;

    public override IEnumerator ExecuteInternal()
    {
        Texture2D knifeSprite = CustomisationContainer.Instance.GetKnifeCustomisation();
        Texture2D objectToCutSprite = CustomisationContainer.Instance.GetObjectToCutCustomisation();

        _knifeImage.texture = knifeSprite;
        _objectToCutImage.texture = objectToCutSprite;

        RawImagePreserveAspect knifePA = _knifeImage.GetComponent<RawImagePreserveAspect>();
        RawImagePreserveAspect objectToCutPA = _objectToCutImage.GetComponent<RawImagePreserveAspect>();

        knifePA.UpdateSize();
        objectToCutPA.UpdateSize();

        Destroy(knifePA);
        Destroy(objectToCutPA);

        FindFirstObjectByType<MoveAcrossSliceAreaTask>().ResetPosition();

        yield return null;
    }
}
