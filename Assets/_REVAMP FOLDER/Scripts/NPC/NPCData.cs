using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC")]
public class NPCData : ScriptableObject
{
    [Header("Dialogue")]

    //The dialogue on first meet
    public List<DialogueLine> onFirstMeet;

    //Default words
    public List<DialogueLine> defaultDialogue;

    public List<SpecialDayDialogue> specialDayDialogues;

    [System.Serializable]
    public struct SpecialDayDialogue
    {
        public int day; // Specific in-game day for the dialogue
        public List<DialogueLine> dialogue;
    }

    public List<DialogueLine> GetDialogueForDay(int currentDay)
    {
        foreach (var specialDialogue in specialDayDialogues)
        {
            if (specialDialogue.day == currentDay)
            {
                return specialDialogue.dialogue;
            }
        }
        return null;
    }
}
