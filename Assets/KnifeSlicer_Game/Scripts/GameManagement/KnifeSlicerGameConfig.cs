using PapayaPlatform;
using UnityEngine;

namespace KnifeSlicer.GameManagement
{
    [CreateAssetMenu(fileName ="Game Config",menuName ="JGH/Create Game Config")]
    public class KnifeSlicerGameConfig : IPapayaGameConfigurations
    {
        [PapayaGameConfigSlider(2f, 100f)]
        public float knifeBaseSpeed = 15f;

        [PapayaGameConfigSlider(1f, 5f)]
        public float knifeEasyMultiplier = 1f;
        [PapayaGameConfigSlider(1f, 5f)]
        public float knifeMediumMultiplier = 2f;
        [PapayaGameConfigSlider(1f, 5f)]
        public float knifeHardMultiplier = 3f;

        public float GetDifficultyValue(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.EASY:
                    return knifeEasyMultiplier;
                case GameDifficulty.NORMAL:
                    return knifeMediumMultiplier;
                case GameDifficulty.HARD:
                    return knifeHardMultiplier;
                default:
                    return 1f;
            }
        }

    }
}