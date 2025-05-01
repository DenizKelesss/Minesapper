using System;

public interface IMinigame
{
    // Keep in mind for future
    // onSuccess: callback if the player succeeds  
    // onFailure: callback if the player fails  
    // customTime: optional override of default timer
    
    void StartMinigame(Action onSuccess, Action onFailure, float customTime = -1f);
}
