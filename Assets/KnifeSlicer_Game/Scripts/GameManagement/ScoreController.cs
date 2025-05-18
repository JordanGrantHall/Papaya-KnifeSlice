using KnifeSlicer.Core;
using KnifeSlicer.Core.Events;
using PapayaPlatform;
using UnityEngine;
namespace KnifeSlicer.GameManagement
{
    public class ScoreController : BaseBehaviour, IPapayaScoreComponents
    {
        [PapayaScoreComponentName("Cutting Score")]
        public int cuttingScore;

        public int GetTotalScore() => cuttingScore;

        [Topic(GameEventIds.ON_KNIFE_CUT)]
        public void VerticalLineCutterBehaviour_OnObjectCut(object sender, Transform obj)
        {
            cuttingScore++;
            PapayaSDKManager.Instance.UpdateScore(this);
        }

    }
}
