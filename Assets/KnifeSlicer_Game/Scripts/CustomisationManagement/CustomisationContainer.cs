using KnifeSlicer.Core.Singleton;
using KnifeSlicer.Customisation;
using KnifeSlicer.Customisation.Utilities;
using PapayaPlatform;
using PapayaPlatform.GameCustomization;
using UnityEngine;

public class CustomisationContainer : MonoSingleton<CustomisationContainer>
{
    public LocalGallery _localGallery;
    public GameResources _gameResources;
    public GameCustomizationData _gameCustomizationData;

    public Texture2D _objectToCutTexture;
    public Texture2D _knifeSelectedTexture;
    public AudioClip _knifeSliceSoundFX;
    public GameDifficulty _gameDifficulty;

    public void SetCustomisationRequirements()
    {
        GameObject resourcesObject = new GameObject();

        if (PapayaSDKManager.Instance != null)
        {
            resourcesObject = Instantiate(PapayaSDKManager.Instance.AssetBundlesManager.LoadAsset<GameObject>("knifeslicer_game",
                "Assets/KnifeSlicer_Game/Prefabs/Customisation/CustomisationRequirements.prefab"));
        }

        if(PapayaSDKGameCustomizationManager.Instance != null)
        {
            resourcesObject = Instantiate(PapayaSDKGameCustomizationManager.Instance.AssetBundlesManager.LoadAsset<GameObject>("knifeslicer_game",
                "Assets/KnifeSlicer_Game/Prefabs/Customisation/CustomisationRequirements.prefab"));
        }

        _localGallery = resourcesObject.GetComponent<LocalGallery>();
        _gameResources = resourcesObject.GetComponent<GameResources>();
    }

    public void SetCustomisationGameRequirements()
    {
        PapayaSDKManager.Instance.SetLocalGameGallery(_localGallery);

        _gameCustomizationData = PapayaSDKManager.Instance.GetGameCustomizationData();
    }

    public Texture2D GetKnifeCustomisation()
    {
        if (_knifeSelectedTexture != null)
            return _knifeSelectedTexture;
        else
        {
            _knifeSelectedTexture = _gameCustomizationData.GetImage(KnifeSlicerCustomisationIds.KNIFE_SELECTION);
            return _knifeSelectedTexture;
        }
    }

    public Texture2D GetObjectToCutCustomisation()
    {
        if (_objectToCutTexture != null)
            return _objectToCutTexture;
        else
        {
            _objectToCutTexture = _gameCustomizationData.GetImage(KnifeSlicerCustomisationIds.OBjECT_TO_CUT_SELECTION);
            return _objectToCutTexture;
        }
    }

    public AudioClip GetKnifeSliceSoundFX()
    {
        if (_knifeSliceSoundFX != null)
            return _knifeSliceSoundFX;
        else
        {
            _knifeSliceSoundFX = _gameCustomizationData.GetAudioClip(KnifeSlicerCustomisationIds.CUTTING_SOUND_SELECTION);
            return _knifeSliceSoundFX;
        }
    }

    public LocalGallery GetLocalGallery()
    {
        if (_localGallery == null)
            SetCustomisationRequirements();

        return _localGallery;
    }

    public GameResources GetGameResources()
    {
        if (_gameResources == null)
            SetCustomisationRequirements();
        return _gameResources;
    }

    public GameDifficulty GetGameDifficulty() => _gameCustomizationData.GameDifficulty;
}
