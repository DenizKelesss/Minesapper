using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;
    public GameObject tutorialSlides;
    public Image[] tutorialClips;
    public float[] clipDurations;
    public Button nextTutClipButton;
    public Button prevTutClipButton;

    [Header("Clean House")]
    public GameObject blockToDelete;

    private int currentClip = 0;
    private Coroutine autoAdvanceClipsCoroutine;
    private bool tutorialCompleted = false;

    // 🔸 Static flag to remember across restarts
    public static bool tutorialAlreadyCompleted = false;

    void Start()
    {
        // If tutorial already done (e.g., after restart), clean up immediately
        if (tutorialAlreadyCompleted)
        {
            if (blockToDelete) Destroy(blockToDelete);
            if (tutorialPanel) Destroy(tutorialPanel);
            if (tutorialSlides) Destroy(tutorialSlides);
            Destroy(this);
            return;
        }

        if (tutorialPanel)
        {
            nextTutClipButton.onClick.AddListener(SkipToNextClip);
            prevTutClipButton.onClick.AddListener(SkipToPreviousClip);
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

        prevTutClipButton.interactable = (clipIndex > 0);

        if (clipIndex < tutorialClips.Length - 1)
        {
            nextTutClipButton.interactable = true;
        }
        else
        {
            nextTutClipButton.interactable = true;
            nextTutClipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        }

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

    public void EndTutorial()
    {
        if (tutorialCompleted) return;
        tutorialCompleted = true;

        tutorialAlreadyCompleted = true; // Remember this for future reloads

        if (blockToDelete) Destroy(blockToDelete);

        if (autoAdvanceClipsCoroutine != null)
            StopCoroutine(autoAdvanceClipsCoroutine);

        if (tutorialPanel) Destroy(tutorialPanel);
        if (tutorialSlides) Destroy(tutorialSlides);

        Debug.Log("Tutorial Completed!");
    }
}
