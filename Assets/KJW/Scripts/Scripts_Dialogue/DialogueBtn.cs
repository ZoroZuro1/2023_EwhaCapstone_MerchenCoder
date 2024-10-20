using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueBtn : MonoBehaviour
{
    //플레이어 위치 저장을 위한 변수//
    Button dialogueBubbleBtn;
    GameObject player;
    GameManager.PlayerData.playerPosition nowPlayerPosition;


    Dialogue dialogue;


    //0603 : 말풍선 가이드 화살표 추가
    private GuideArrowEffect guideArrow;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBubbleBtn = GetComponent<Button>();
        player = GameObject.FindWithTag("Player");
        if (transform.childCount > 0)
        {
            transform.GetChild(0).TryGetComponent<GuideArrowEffect>(out guideArrow); //0603
        }


        if (dialogueBubbleBtn != null)
        {
            dialogueBubbleBtn.interactable = false;
            dialogueBubbleBtn.onClick.RemoveAllListeners();
            dialogueBubbleBtn.onClick.AddListener(SavePlayerPosition);
        }
        dialogue = GetComponent<Dialogue>();
    }



    public void DialogueBtnDown()
    {
        guideArrow.IsClicked = true;
        if (guideArrow != null)
            guideArrow.gameObject.SetActive(false);

        dialogue.DialogueStart();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            dialogueBubbleBtn.interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            dialogueBubbleBtn.interactable = false;
        }
    }

    private void SavePlayerPosition()
    {
        string thisSceneName = SceneManager.GetActiveScene().name;
        string chapter = thisSceneName.Substring(0, 1);
        GameManager.Instance.playerData.chapterCurrentScene[int.Parse(chapter) - 1] = thisSceneName;

        //포지션 기록

        nowPlayerPosition.x = player.transform.localPosition.x;
        nowPlayerPosition.y = player.transform.localPosition.y;
        nowPlayerPosition.z = player.transform.localPosition.z;
        GameManager.Instance.playerData.playLog[thisSceneName] = nowPlayerPosition;
        GameManager.Instance.SavePlayerData();
    }


}
