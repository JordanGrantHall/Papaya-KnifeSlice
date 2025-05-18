using KnifeSlicer.Core;
using KnifeSlicer.Core.Events;
using KnifeSlicer.Tasks;
using KnifeSlicer.Utilities;
using PapayaPlatform;
using PapayaPlatform.GameCustomization;
using System.Linq;
using UnityEngine;
namespace KnifeSlicer.GameManagement
{
    public class GameManager : BaseBehaviour
    {

        public TaskManager _setupTaskManager;
        public TaskManager _startGameTaskManager;

        private KnifeSlicerGameConfig _gameConfig;
        private PapayaSDKManager _papayaSDKManager;

        public KnifeSlicerGameConfig GameConfig => _gameConfig;

        private void Awake()
        {
            _papayaSDKManager = PapayaSDKManager.Instance;
            PapayaLifeCycleGame papayaLifeCycleGame = new PapayaLifeCycleGame(this);
            _papayaSDKManager.InitGame(papayaLifeCycleGame);
        }

        public void CountdownStarted()
        {
            Debug.Log("Countdown started");
        }

        public void GoalAchieved()
        {
            Debug.Log("Goal achieved");
        }

        public void LoadGame(IPapayaGameConfigurations gameConfig)
        {
            _gameConfig = (KnifeSlicerGameConfig)gameConfig;
            CustomisationContainer.Instance.SetCustomisationRequirements();
            CustomisationContainer.Instance.SetCustomisationGameRequirements();
            _papayaSDKManager.SetLocalGameGallery(CustomisationContainer.Instance.GetLocalGallery());

            StartCoroutine(_setupTaskManager.Execute());

            SetSettings();
            SetDefaultSettings();

            _papayaSDKManager.ReportLoadProgress(1);
        }

        public void SetSettings()
        {

        }

        public void SetDefaultSettings()
        {

        }

        public void PauseGame()
        {
            Debug.Log("Game paused");
        }

        public void ResumeGame()
        {
            Debug.Log("Game resumed");
        }

        public void StartGame()
        {
            Debug.Log("Game started");
            StartCoroutine(_startGameTaskManager.Execute());
        }

        public void TerminateGame()
        {
            Debug.Log("Game terminated");
        }

        public void UnloadGame()
        {
            UtilityFuncs.FindInterfacesInScene<IPapayaGameSpecific>().ToList().ForEach(i => i.Unload());
        }
        public void SetGameVolume(float volume)
        {
        }

        public void SetHaptics(bool playHaptics)
        {
        }

        public void SetMusicVolume(float volume)
        {
        }

        [Topic(GameEventIds.ON_GAME_FINISHED)]
        public void OnGameEndedAnnounced(object sender)
        {
            ScoreController scoreController = FindFirstObjectByType<ScoreController>();
            _papayaSDKManager.GameComplete(scoreController);
        }
    }
}