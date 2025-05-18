using DG.Tweening;
using KnifeSlicer.GameManagement;
using KnifeSlicer.Tasks;
using PapayaPlatform;
using System.Collections;
using UnityEngine;

public class MoveAcrossSliceAreaTask : TaskBase
{
    public KnifeCutter _knifeCutter;
    public Transform _knife;
    public Transform _leftEdge;
    public Transform _rightEdge;

    public override IEnumerator ExecuteInternal()
    {
        _knifeCutter.enabled = true;

        _knife.transform.position = _leftEdge.position;

        float knifeDuration = GetKnifeTravelTime(CustomisationContainer.Instance.GetGameDifficulty());

        Tween moveTween = _knife
        .DOMove(_rightEdge.position, knifeDuration)
        .SetEase(Ease.Linear);

        // Yield until it’s done
        yield return moveTween.WaitForCompletion();

        _knifeCutter.enabled = false;
    }

    public float GetKnifeTravelTime(GameDifficulty difficulty)
    {
        KnifeSlicerGameConfig gameConfig = FindFirstObjectByType<GameManager>().GameConfig;

        float baseValue = gameConfig.knifeBaseSpeed;
        float difficultyValue = gameConfig.GetDifficultyValue(difficulty);

        return baseValue / difficultyValue;

    }

    public void ResetPosition()
    {
        _knife.transform.position = _leftEdge.position + (Vector3.right * 5f);
        _knifeCutter.enabled = false;
    }
}
