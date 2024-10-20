using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RefreshCanvas : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    public Vector3 originVecor3;

    public GameObject playerChatBubble;

    public bool isPlayerPositionReset = true;
    [SerializeField]
    private AutoAudioSetting autoAudioSetting;
    private void Start()
    {
        if (autoAudioSetting == null)
        {
            autoAudioSetting = GetComponentInParent<AutoAudioSetting>(true);
        }
        player = gameObject.transform.Find("Result_img").GetChild(0).gameObject;
        if (player != null)
        {
            if (isPlayerPositionReset)
                originVecor3 = player.transform.localPosition;
            animator = player.GetComponentInChildren<Animator>(true);
        }
    }

    public void PlayRunErrorSound()
    {
        autoAudioSetting.OnClickSound_Index(10);
    }
    public void PlayRunCompleteSound()
    {
        autoAudioSetting.OnClickSound_Index(5);
    }


    private void LoadOriginPosition()
    {
        if (player != null && isPlayerPositionReset)
        {
            player.transform.localPosition = originVecor3;
        }
    }

    public void stopPlaying()
    {
        if (autoAudioSetting.AudioSource.isPlaying)
        {
            autoAudioSetting.AudioSource.Stop();
            Debug.Log("실행중인 모든 효과음 중단");
        }
        //소리설정
        autoAudioSetting.OnClickSound_Index(0);


        //애니메이션 및 행동 말풍선 초기화
        MonoBehaviour[] allScripts = Object.FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour script in allScripts)
        {
            if(script.gameObject.name != "Canvas_Tuto")
                script.StopAllCoroutines();
        }
        LoadOriginPosition();

        ResetChatBubble();

        //에러 메시지 박스 초기화
        GetComponent<RunErrorMsg>().InActiveErrorMsg();


        //애니메이션 중지
        //action 애니메이션
        if (animator == null)
        {
            animator = player.GetComponent<Animator>();

        }
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            Debug.Log("Parameter Name: " + parameter.name + ", Type: " + parameter.type);

            // 파라미터의 타입에 따라 값을 가져올 수 있음
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                print(parameter.name);
                animator.SetBool(parameter.name, false);
            }
            else if (parameter.type == AnimatorControllerParameterType.Float)
            {
                animator.SetFloat(parameter.name, 0);
            }
            else if (parameter.type == AnimatorControllerParameterType.Int)
            {
                animator.SetInteger(parameter.name, 0);
            }
        }
        //action 애니메이션 말풍선 끄기
        Transform result_img = transform.GetChild(2);
        foreach (Transform character in result_img)
        {
            Debug.Log(character.name);
            OffActionBubble(character);
        }

        //결과 애니메이션 초기화
        if (GetComponent<ControlAnimation>().result_anim != null)
        {
            GetComponent<ControlAnimation>().result_anim.SetInteger("Control", 0);
            //사운드 초기화
            GetComponent<ControlAnimation>().animationAudioControl.StopAnimationSound();

        }

        this.gameObject.SetActive(false);
    }

    public void OffActionBubble(Transform character)
    {
        print(character.name);
        if (character.tag != "NPC" && character.tag != "Player") return;
        if (character.childCount == 0 || character.childCount == 1 && character.GetChild(0).childCount == 0)
        {
            Debug.Log("don't have action bubble");
        }
        else
        {
            foreach (Transform bubble in character.GetChild(0))
            {
                Debug.Log(bubble.name);
                if (bubble.CompareTag("actionBubble"))
                {
                    Debug.Log(bubble.name);
                    bubble.gameObject.SetActive(false);
                }

            }
        }
    }
    public void ResetChatBubble()
    {
        GameObject canvas = transform.parent.GetChild(2).gameObject;
        foreach (Transform child in canvas.transform)
        {
            child.GetComponentInChildren<TextMeshProUGUI>(true).text = null;
            Debug.Log("말풍선 초기화, 비활성화");
            child.gameObject.SetActive(false);
        }
    }
}