using PapayaPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CameraSetter : MonoBehaviour
{
    [SerializeField]
    private Color _noCamColor = Color.white;

    private RawImage _camImage;

    private void Awake()
    {
        _camImage = GetComponent<RawImage>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PapayaSDKManager.Instance != null);

        Texture camTexture = PapayaSDKManager.Instance.GetPlayerCamTexture();

        if(camTexture != null)
        {
            _camImage.texture = camTexture;
        }
        else
        {
            _camImage.color = _noCamColor;
        }
    }
}
