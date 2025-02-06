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
}
