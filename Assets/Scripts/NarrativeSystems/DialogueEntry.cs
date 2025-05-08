using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public string characterName;
    [TextArea(3, 10)]
    public string dialogueLine;
    public float duration = 3f; // default time to auto-skip
}