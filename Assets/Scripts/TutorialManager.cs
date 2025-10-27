using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;          // The main panel that holds the tutorial UI
    public Image[] tutorialClips;             // Array of tutorial images or steps
    public float[] clipDurations;             // Duration each tutorial clip stays before auto-advancing
    public Button nextTutClipButton;          // Button to manually skip to the next tutorial clip

    private int currentClip = 0;              // Index of the current clip
    private Coroutine autoAdvanceClipsCoroutine;
    private bool tutorialCompleted = false;   // Prevent multiple calls to EndTutorial()

    void Start()
    {
        if (tutorialPanel)
        {
            nextTutClipButton.onClick.AddListener(SkipToNextClip);

            ShowClip(currentClip);
        }
    }

    void ShowClip(int clipIndex)
    {
        if (clipIndex >= tutorialClips.Length)
        {
            EndTutorial();
            return;
        }

        for (int i = 0; i < tutorialClips.Length; i++)
            tutorialClips[i].gameObject.SetActive(i == clipIndex);

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

    void GoToNextClip()
    {
        currentClip++;
        ShowClip(currentClip);
    }

    void EndTutorial()
    {
        if (tutorialCompleted) return;
        tutorialCompleted = true;

        if (autoAdvanceClipsCoroutine != null)
            StopCoroutine(autoAdvanceClipsCoroutine);

        if (tutorialPanel != null)
            Destroy(tutorialPanel);

        Debug.Log("Tutorial Completed!");

    }
}
