using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;          // The main panel that holds the tutorial UI
    public GameObject tutorialSlides;         // Parent object containing the slide images
    public Image[] tutorialClips;             // Array of tutorial images or steps
    public float[] clipDurations;             // Duration each tutorial clip stays before auto-advancing
    public Button nextTutClipButton;          // Button to manually skip to the next tutorial clip
    public Button prevTutClipButton;          // Button to go back to the previous tutorial clip

    private int currentClip = 0;              // Index of the current clip
    private Coroutine autoAdvanceClipsCoroutine;
    private bool tutorialCompleted = false;   // Prevent multiple calls to EndTutorial()

    [Header("Clean House")]
    public GameObject blockToDelete;


    void Start()
    {
        if (tutorialPanel)
        {
            nextTutClipButton.onClick.AddListener(SkipToNextClip);
            prevTutClipButton.onClick.AddListener(SkipToPreviousClip);

            ShowClip(currentClip);
        }
    }

    void ShowClip(int clipIndex)
    {
        // If we’ve gone past the final clip, end tutorial
        if (clipIndex >= tutorialClips.Length)
        {
            EndTutorial();
            return;
        }

        // Show only the current clip
        for (int i = 0; i < tutorialClips.Length; i++)
            tutorialClips[i].gameObject.SetActive(i == clipIndex);

        // Enable/disable buttons
        prevTutClipButton.interactable = (clipIndex > 0);

        // Keep the Next button active on the final slide, but rename it "Finish"
        if (clipIndex < tutorialClips.Length - 1)
        {
            nextTutClipButton.interactable = true;
            /*nextTutClipButton.GetComponentInChildren<Text>().text = "SKIP";*/
        }
        else
        {
            nextTutClipButton.interactable = true; // Still clickable!
            nextTutClipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        }

        // Stop and restart the auto-advance
        if (autoAdvanceClipsCoroutine != null)
            StopCoroutine(autoAdvanceClipsCoroutine);

        autoAdvanceClipsCoroutine = StartCoroutine(AutoNextClip(clipDurations[clipIndex]));
    }

    IEnumerator AutoNextClip(float delay)
    {
        yield return new WaitForSeconds(delay);
        GoToNextClip();
    }

    void SkipToNextClip()
    {
        if (autoAdvanceClipsCoroutine != null)
            StopCoroutine(autoAdvanceClipsCoroutine);

        GoToNextClip();
    }

    void SkipToPreviousClip()
    {
        if (autoAdvanceClipsCoroutine != null)
            StopCoroutine(autoAdvanceClipsCoroutine);

        GoToPreviousClip();
    }

    void GoToNextClip()
    {
        currentClip++;

        // If we're at the last clip, this ends the tutorial
        if (currentClip >= tutorialClips.Length)
        {
            EndTutorial();
            return;
        }

        ShowClip(currentClip);
    }

    void GoToPreviousClip()
    {
        currentClip--;
        if (currentClip < 0) currentClip = 0;
        ShowClip(currentClip);
    }

    void EndTutorial()
    {
        if (tutorialCompleted) return;
        tutorialCompleted = true;

        Destroy(blockToDelete);

        if (autoAdvanceClipsCoroutine != null)
            StopCoroutine(autoAdvanceClipsCoroutine);

        if (tutorialPanel != null)
            Destroy(tutorialPanel);

        if (tutorialSlides != null)
            Destroy(tutorialSlides);

        Debug.Log("Tutorial Completed!");
    }
}