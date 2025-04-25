using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComicIntro : MonoBehaviour
{
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

        // Trigger gameplay start here
    }
}