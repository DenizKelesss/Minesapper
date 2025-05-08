using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI charNameText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Button skipButton; // Link your UI Button here

    private List<DialogueEntry> currentDialogue;
    private int currentIndex = 0;
    private Coroutine dialogueCoroutine;
    private bool isDialogueActive = false;

    private FirstPersonPlayer player;


    private void Start()
    {
        dialoguePanel.SetActive(false);
        skipButton.onClick.AddListener(SkipDialogueLine);
    }

    public void StartDialogue(List<DialogueEntry> dialogueSequence)
    {
        player = FindFirstObjectByType<FirstPersonPlayer>();
        if (player != null) player.canMove = false;

        if (isDialogueActive) return;

        currentDialogue = dialogueSequence;
        currentIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        PlayCurrentDialogue();
    }

    private void PlayCurrentDialogue()
    {
        if (currentIndex >= currentDialogue.Count)
        {
            EndDialogue();
            return;
        }

        var entry = currentDialogue[currentIndex];
        charNameText.text = entry.characterName;
        dialogueText.text = entry.dialogueLine;

        // Start timer for auto-skip
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = StartCoroutine(AutoSkipAfterDelay(entry.duration));
    }

    private IEnumerator AutoSkipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SkipDialogueLine();
    }

    public void SkipDialogueLine()
    {
        if (!isDialogueActive) return;

        currentIndex++;
        PlayCurrentDialogue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        player = FindFirstObjectByType<FirstPersonPlayer>();
        if (player != null) player.canMove = true;
    }

    private void Update()
    {
        // Optional: Allow keyboard skipping (e.g., press E or Space)
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            SkipDialogueLine();
        }
    }
}