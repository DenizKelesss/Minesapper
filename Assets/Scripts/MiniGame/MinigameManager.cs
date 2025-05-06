using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [Tooltip("Assign any MonoBehaviour that implements IMinigame here")]
    public List<MonoBehaviour> minigameScripts;


    //serialized to appear in editor - used to force certain games to load, for testing purposes. < 0 causes standard function
    [SerializeField]
    private int gameForce = -1;

    // Randomly launches one of the registered minigames - randomness is never true random.
    public void LaunchRandomMinigame(Action onSuccess, Action onFailure, float customTime = -1f)
    {
        if (minigameScripts == null || minigameScripts.Count == 0)
        {
            Debug.LogError("MinigameManager: No minigames assigned!");
            return;
        }

        int idx = UnityEngine.Random.Range(0, minigameScripts.Count);
        if(gameForce >= 0)
            idx = gameForce;
        var chosen = minigameScripts[idx] as IMinigame;
        if (chosen != null)
        {
            chosen.StartMinigame(onSuccess, onFailure, customTime);
        }
        else
        {
            Debug.LogError($"MinigameManager: Script at index {idx} does not implement IMinigame.");
        }
    }
}
