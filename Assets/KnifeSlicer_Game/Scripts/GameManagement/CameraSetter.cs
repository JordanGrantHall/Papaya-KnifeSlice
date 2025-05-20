using PapayaPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CameraSetter : MonoBehaviour
{
    [SerializeField]
    private GameObject noShowObject;

    private RawImage _camImage;


    private void Awake()
    {
        _camImage = GetComponent<RawImage>();
        noShowObject.gameObject.SetActive(false);
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
            noShowObject.gameObject.SetActive(true);
        }
    }
}
