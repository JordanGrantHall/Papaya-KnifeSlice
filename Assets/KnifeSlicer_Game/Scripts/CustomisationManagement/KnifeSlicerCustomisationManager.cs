using KnifeSlicer.Core;
using KnifeSlicer.Customisation.Utilities;
using KnifeSlicer.GameManagement;
using KnifeSlicer.General.Utility;
using PapayaPlatform;
using PapayaPlatform.GameCustomization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KnifeSlicer.Customisation
{

    public class KnifeSlicerCustomisationManager : BaseBehaviour, IpapayaGameCustomization
    {
        private PapayaSDKGameCustomizationManager _customisationSDK;

        public GameResources GameResources => CustomisationContainer.Instance._gameResources;
        public LocalGallery LocalGallery => CustomisationContainer.Instance._localGallery;

        private void Awake()
        {
            _customisationSDK = PapayaSDKGameCustomizationManager.Instance;
            _customisationSDK.InitGame(this);
        }

        public void LoadGame(IPapayaGameConfigurations gameConfig)
        {
            CustomisationContainer.Instance.SetCustomisationRequirements();
            var gcScreen = _customisationSDK.CreateGameCustomizationScreen(LocalGallery);

            SetupGameplayCustomisation(gcScreen);
            SetupArtCustomisation(gcScreen);
            SetupAudioCustomisation(gcScreen);

            _customisationSDK.ReportLoadProgress(1);
        }

        public void SetupGameplayCustomisation(ConsistentGameCustomizationScreen gcScreen)
        {
            var gameplayCategory = gcScreen.AddCategory(KnifeSlicerCustomisationIds.GAMEPLAY_CATEGORY, "Gameplay", GameCustomizationIcon.Joystick);

            gameplayCategory.AddGameDifficultySwitch(GameDifficulty.EASY, (difficulty) => {  });
        }

        public void SetupArtCustomisation(ConsistentGameCustomizationScreen gcScreen)
        {
            var artCategory = gcScreen.AddCategory(KnifeSlicerCustomisationIds.ART_CATEGORY, "Art", GameCustomizationIcon.Brush);

            artCategory.AddImageChooser(KnifeSlicerCustomisationIds.OBjECT_TO_CUT_SELECTION, "Cutting Object", new ImageChooserOptions
            {
                AllowUpload = true,
                AllowNone = false,
                PresetImagesIDs = GetImageIdsByPrefix("cuttingobject-"),
                LoadedImageMask = GameResources.HorizontalMask,
                LoadedImageMaskOutline = GameResources.HorizontalMaskOutline,
            }, (texture) => Publish(KnifeSlicerCustomisationIds.ON_OBjECT_TO_CUT_SELECTED, texture));

            artCategory.AddImageChooser(KnifeSlicerCustomisationIds.KNIFE_SELECTION, "Knifes", new ImageChooserOptions
            {
                AllowUpload = true,
                AllowNone = false,
                PresetImagesIDs = GetImageIdsByPrefix("knife-"),
                LoadedImageMask = GameResources.VerticalMask,
                LoadedImageMaskOutline = GameResources.VerticalMaskOutline,
            }, (texture) => Publish(KnifeSlicerCustomisationIds.ON_KNIFE_CUSTOMISATION_SELECTED, texture));
        }

        public void SetupAudioCustomisation(ConsistentGameCustomizationScreen gcScreen)
        {
            var audioCategory = gcScreen.AddCategory(KnifeSlicerCustomisationIds.SOUND_CATEGORY, "Audio", GameCustomizationIcon.MusicNote);

            audioCategory.AddAudioChooser(KnifeSlicerCustomisationIds.CUTTING_SOUND_SELECTION, "Cutting Sound", new AudioChooserOptions
            {
                AllowUpload = true,
                AllowNone = true,
                PresetAudioClipsIDs = GetAudioIdsByPrefix(),
                RecordingMaxTime = 3,
                DefaultSelectedIndex = 0
            }, (clip) => Publish(KnifeSlicerCustomisationIds.ON_CUTTING_SOUND_CUSTOMISATION_SELECTED));
        }

        public void UnloadGame()
        {
        }

        public List<string> GetImageIdsByPrefix(string prefix = null)
        {
            if(string.IsNullOrEmpty(prefix))
                return LocalGallery.Images
                    .Select(i => i.ID)
                    .ToList();

            return LocalGallery.Images
                .Where(i => i.ID.StartsWith(prefix))
                .Select(i => i.ID)
                .ToList();
        }

        public List<string> GetAudioIdsByPrefix(string prefix = null)
        {
            if(string.IsNullOrEmpty(prefix))
                return LocalGallery.Sounds
                    .Select(i => i.ID)
                    .ToList();

            return LocalGallery.Sounds
                .Where(i => i.ID.StartsWith(prefix))
                .Select(i => i.ID)
                .ToList();
        }
    }

    public static class KnifeSlicerCustomisationIds
    {
        ////////////////////////////////////////////////////////////////////////////////////
        ///                                  CATEGORIES                                  ///
        ////////////////////////////////////////////////////////////////////////////////////
        public const string GAMEPLAY_CATEGORY = "gameplay-category";
        public const string ART_CATEGORY = "art-category";
        public const string SOUND_CATEGORY = "sound-category";

        ////////////////////////////////////////////////////////////////////////////////////
        ///                               SELECTION TYPES                                ///
        ////////////////////////////////////////////////////////////////////////////////////
        public const string KNIFE_SELECTION = "knife-selection";
        public const string OBjECT_TO_CUT_SELECTION = "object-to-cut-selection";
        public const string CUTTING_SOUND_SELECTION = "cutting-sound-selection";

        ////////////////////////////////////////////////////////////////////////////////////
        ///                             EVENT ANNOUNCEMENTS                              ///
        ////////////////////////////////////////////////////////////////////////////////////
        public const string ON_DIFFICULTY_SELECTED = "on-difficulty-selected";
        public const string ON_KNIFE_CUSTOMISATION_SELECTED = "on-knife-customisation-selected";
        public const string ON_OBjECT_TO_CUT_SELECTED = "object-to-cut-selected";
        public const string ON_CUTTING_SOUND_CUSTOMISATION_SELECTED = "on-cutting-sound-customisation-selected";
    }
}