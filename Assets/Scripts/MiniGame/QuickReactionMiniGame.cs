// Assets/Scripts/QuickReactionMinigame.cs
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickReactionMiniGame : MonoBehaviour, IMinigame
{
    public GameObject minigameUI;
    public TextMeshProUGUI instructionText;
    public Button reactionButton;

    private Action onSuccess;
    private Action onFailure;

    private bool canReact = false;
    private bool hasReacted = false;
    private float reactionStartTime;
    private FirstPersonPlayer player;


    public void StartMinigame(Action successCallback, Action failureCallback, float customTime = -1f)
    {
        player = FindFirstObjectByType<FirstPersonPlayer>();
        if (player != null) player.canMove = false;

        onSuccess = successCallback;
        onFailure = failureCallback;
        minigameUI.SetActive(true);
        instructionText.text = "Wait for it...";
        canReact = false;
        hasReacted = false;

        reactionButton.onClick.RemoveAllListeners();
        reactionButton.onClick.AddListener(OnPlayerClicked);

        float delay = UnityEngine.Random.Range(1.5f, 3.5f);
        StartCoroutine(StartReactionWindow(delay));
    }

    private IEnumerator StartReactionWindow(float delay)
    {
        yield return new WaitForSeconds(delay);
        instructionText.text = "NOW!";
        canReact = true;
        reactionStartTime = Time.time;

        // Fail automatically after 1.5 seconds if no click
        yield return new WaitForSeconds(1.5f);
        if (!hasReacted)
        {
            Fail();
        }
    }

    private void OnPlayerClicked()
    {
        if (hasReacted) return;

        if (!canReact)
        {
            instructionText.text = "Too early!";
            Fail();
        }
        else
        {
            hasReacted = true;
            float reactionTime = Time.time - reactionStartTime;
            if (reactionTime <= 1f) // success if clicked within 1 second
            {
                instructionText.text = $"Success! ({reactionTime:F2}s)";
                Success();
            }
            else
            {
                instructionText.text = $"Too slow! ({reactionTime:F2}s)";
                Fail();
            }
        }
    }

    private void Success()
    {
        Invoke(nameof(EndWithSuccess), 1f);
        var um = FindFirstObjectByType<UpgradeManager>();
        if (um != null) um.GainXP(UnityEngine.Random.Range(5, 11));
    }

    private void Fail()
    {
        hasReacted = true;
        Invoke(nameof(EndWithFailure), 1f);
        var um = FindFirstObjectByType<UpgradeManager>();
        int dmg = (um != null) ? um.GetFailDamage() : 3;
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null) ph.DecreaseHealth(dmg);
    }

    private void EndWithSuccess()
    {
        minigameUI.SetActive(false);
        onSuccess?.Invoke();
        if (player != null) player.canMove = true;

    }

    private void EndWithFailure()
    {
        minigameUI.SetActive(false);
        onFailure?.Invoke();
        if (player != null) player.canMove = true;
    }
}
