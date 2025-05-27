using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComicIntro : MonoBehaviour
{
    public static bool comicIntroHasPlayed = false; // this should make sure the Comic sections are only played once.

    public GameObject comicPanel;
    public GameObject comicPagesObject;
    public Image[] comicPages;
    public float[] pageDurations;
    public AudioClip[] voiceovers;
    public AudioSource audioSource;
    public Button nextButton;

    private int currentPage = 0;
    private Coroutine autoAdvanceCoroutine;

    void Start()
    {
        if (comicIntroHasPlayed)
        {
            comicPanel.SetActive(false);
            comicPagesObject.SetActive(false);
            Destroy(this.gameObject);
            return;
        }

        comicIntroHasPlayed = true;

        comicPanel.SetActive(true);
        nextButton.onClick.AddListener(SkipToNextPage);
        ShowPage(currentPage);
    }

    void ShowPage(int pageIndex)
    {
        if (pageIndex >= comicPages.Length)
        {
            EndIntro();
            return;
        }

        for (int i = 0; i < comicPages.Length; i++)
            comicPages[i].gameObject.SetActive(i == pageIndex);

        if (voiceovers != null && voiceovers.Length > pageIndex)
        {
            audioSource.clip = voiceovers[pageIndex];
            audioSource.Play();
        }

        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
        autoAdvanceCoroutine = StartCoroutine(AutoNextPage(pageDurations[pageIndex]));
    }

    IEnumerator AutoNextPage(float delay)
    {
        yield return new WaitForSeconds(delay);
        GoToNextPage();
    }

    void SkipToNextPage()
    {
        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
        GoToNextPage();
    }

    void GoToNextPage()
    {
        currentPage++;
        ShowPage(currentPage);
    }

    void EndIntro()
    {
        if (autoAdvanceCoroutine != null) StopCoroutine(autoAdvanceCoroutine);
        audioSource.Stop();
        Destroy(comicPanel);
        Destroy(comicPagesObject);

        //leads to the scene, the same scene
    }
}