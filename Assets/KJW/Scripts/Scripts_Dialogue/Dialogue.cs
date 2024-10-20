using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [NonSerialized] public DialogueSystem dialogueSystem;
    public enum Face { Normal, Smile, Sad, Cry, Angry, Surprise }

    [Serializable]
    public class EachDialogue
    {
        public int diaID;
        public int index;
        public string speaker;
        public string face;
        [TextArea()] public string dialogueText;
        public Speaker GetSpeaker()
        {
            // Speaker Info Load
            return Resources.Load<Speaker>("Speaker/" + speaker);
        }
        public Face GetFace()
        {
            return (Face)System.Enum.Parse(typeof(Face), face);
        }
    }

    [Serializable]
    public class DialogueContainer
    {
        public EachDialogue[] dialogueList;
    }

    public DialogueContainer dialogueContainer;
    public EachDialogue[] thisIdDialogues;

    // 파일 이름으로 경로를 통해 가져오도록 수정
    private string dialogueJson;
    public string dialogueFileName;
    public int dialogueID;

    private void Start()
    {
        dialogueSystem = GameObject.Find("Canvas_Dialogue").GetComponent<DialogueSystem>();
        // string jsonFilePath = Application.dataPath + "/Data/Dialogue/" + dialogueFileName + ".json";
        string jsonFilePath = Application.persistentDataPath + "/static/Dialogue/" + dialogueFileName + ".json";
        dialogueJson = File.ReadAllText(jsonFilePath);
        string jsonString = "{ \"dialogueList\": " + dialogueJson + "}";
        dialogueContainer = JsonUtility.FromJson<DialogueContainer>(jsonString);
        FindDialogueByID(dialogueID);
    }


    // Need check in Inspector
    public void FindDialogueByID(int targetDiaID)
    {
        int i;
        int j = 0;
        for (i = 0; i < dialogueContainer.dialogueList.Length; i++)
        {
            if (dialogueContainer.dialogueList[i].diaID == targetDiaID)
            {
                break;
            }
        }
        while (i + j + 1 < dialogueContainer.dialogueList.Length)
        {
            // OutOfIndex 방지
            if (dialogueContainer.dialogueList[i + j + 1].diaID == targetDiaID)
            {
                j++;
            }
            else break;
        }
        thisIdDialogues = new EachDialogue[j+1];
        for (int k = 0; k<=j; k++)
        {
            if (i + k < dialogueContainer.dialogueList.Length)
            {
                thisIdDialogues[k] = dialogueContainer.dialogueList[k + i];
            }
            else break;
        }
    }

    // Dialogue
    public void DialogueStart()
    {
        // 플레이어 이름 갈아끼기를 실행 시점으로 옮김
        for (int i = 0; i < thisIdDialogues.Length; i++)
        {
            thisIdDialogues[i].dialogueText = thisIdDialogues[i].dialogueText.Replace("{}", PlayerPrefs.GetString("player_name"));
        }

        if (dialogueSystem == null)
        {
            dialogueSystem = GameObject.Find("Canvas_Dialogue").GetComponent<DialogueSystem>();
        }
        dialogueSystem.nowDialogueList = thisIdDialogues;
        dialogueSystem.dialogues.Add(gameObject);
        dialogueSystem.StartSpeak();
    }
}

