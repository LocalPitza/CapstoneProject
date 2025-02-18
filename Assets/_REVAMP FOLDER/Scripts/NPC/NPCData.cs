using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPC")]
public class NPCData : ScriptableObject
{
    [Header("Dialogue")]

    public ItemData wantItem;

    //The dialogue on first meet
    public List<DialogueLine> onFirstMeet;

    //Default words
    public List<DialogueLine> defaultDialogue;

    public List<DialogueLine> thankYouDialogue;

    public List<SpecialDayDialogue> specialDayDialogues;

    [System.Serializable]
    public struct SpecialDayDialogue
    {
        public int startDay; // The start of the special dialogue period
        public int endDay; // Specific in-game day for the dialogue
        public List<DialogueLine> dialogue;
    }

    public List<DialogueLine> GetDialogueForDay(int currentDay)
    {
        foreach (var specialDialogue in specialDayDialogues)
        {
            if (currentDay >= specialDialogue.startDay && currentDay <= specialDialogue.endDay)
            {
                return specialDialogue.dialogue;
            }
        }
        return null;
    }
}
