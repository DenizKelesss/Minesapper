using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public List<DialogueEntry> dialogueSequence;

    public GameObject interactPrompt;
    private bool playerInRange = false;
    private DialogueManager dialogueManager;

    private void Start()
    {
        interactPrompt.SetActive(false);
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactPrompt.SetActive(false);
            dialogueManager.StartDialogue(dialogueSequence);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
            playerInRange = false;
        }
    }
}