using KnifeSlicer.GameManagement;
using KnifeSlicer.Tasks;
using System.Collections;
using UnityEngine;

public class AnnounceEndTask : TaskBase
{
    public override IEnumerator ExecuteInternal()
    {
        yield return new WaitForSeconds(1f);
        Publish(GameEventIds.ON_GAME_FINISHED);
        Debug.Log("Task Completed");
    }
}
