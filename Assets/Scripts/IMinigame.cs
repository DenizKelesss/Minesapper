using System;

public interface IMinigame
{
    /// <summary>
    /// Starts the minigame.  
    /// onSuccess: callback if the player succeeds  
    /// onFailure: callback if the player fails  
    /// customTime: optional override of default timer
    /// </summary>
    void StartMinigame(Action onSuccess, Action onFailure, float customTime = -1f);
}
