using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviewButton : MonoBehaviour
{
    public GameObject missionPanel;
    private string missionCode;
    Button button;
    [SerializeField] PlayByAudioManager playByAudioManager;
    // Start is called before the first frame update
    void Start()
    {
        //missionPanel 참조 Public으로 변경
        //missionPanel = GetComponentInParent<Canvas>().transform.GetChild(6).GetChild(1).gameObject;
        missionCode = transform.GetComponentInChildren<TMP_Text>().text;
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(SetMissionDataPanel);

    }


    public void SetMissionDataPanel()
    {
        playByAudioManager.PlaySFXByAudioManager(0);
        //게임 매니져 미션 정보 로드하도록 호출
        GameManager.Instance.LoadMissionData(missionCode);
        //리뷰는 보상 10으로 바꾸기
        GameManager.Instance.changeReward(10);
        //미션 패널 정보 셋팅해주기
        missionPanel.GetComponent<MissionManager>().MissionInfoSetting();
        missionPanel.SetActive(true);
    }
}
