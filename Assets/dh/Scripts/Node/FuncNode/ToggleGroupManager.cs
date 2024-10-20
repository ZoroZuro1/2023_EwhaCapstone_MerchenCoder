using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class ToggleGroupManager : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    public Button button;

    private int flagOption = 0;

    // private Toggle

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        //button.interactable = false;
    }
    private void OnEnable()
    {
        button.GetComponent<FunctionMaker>().selectType = 1;
        if (FlagManager.instance != null) button.interactable = false;
    }
    public void OnToggleValueChagned(bool isOn)
    {
        if (isOn)
        {
            Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
            int selectedToggleCount = 1;
            foreach (Toggle toggle in toggles)
            {
                if (toggle.isOn)
                {
                    break; // 하나라도 선택되었다면 반복문 종료
                }
                else
                {
                    selectedToggleCount++;
                }

            }

            // 선택된 Toggle이 있으면 버튼 활성화, 그렇지 않으면 비활성화
            // 튜토리얼 플래그 추가 240528
            if (FlagManager.instance != null)
            {
                Debug.Log(gameObject.name);
                if (FlagManager.instance.flagStr == "SelectOption" + selectedToggleCount.ToString())
                {
                    if (flagOption == 0) FlagManager.instance.OffFlag();
                    flagOption = selectedToggleCount;
                    FlagManager.instance.flagStr = "SelectOKBtn";
                    button.interactable = selectedToggleCount >= 1 && selectedToggleCount <= 4;
                    button.GetComponent<FunctionMaker>().selectType = selectedToggleCount;
                    Debug.Log("Type: " + button.GetComponent<FunctionMaker>().selectType);
                }
                else if (FlagManager.instance.flagStr == "SelectOKBtn")
                {
                    FlagManager.instance.flagStr = "SelectOption" + flagOption.ToString();
                    OnToggleValueChagned(true);
                }
                else button.interactable = false;
            }
            else
            {
                button.interactable = selectedToggleCount >= 1 && selectedToggleCount <= 4;
                button.GetComponent<FunctionMaker>().selectType = selectedToggleCount;
                Debug.Log("Type: " + button.GetComponent<FunctionMaker>().selectType);
            }
        }
    }

    //close button 눌렀을 때 모달 창 reset
    public void ResetToggleSelection()
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        // toggleGroup.SetAllTogglesOff();
        //SetAllTogglesOff()가 동작을 안해서 수동으로 off... 이유는 모름...

        for (int i = 0; i < toggles.Length; i++)
        {
            if (i == 0)
                toggles[0].isOn = true;

            if (toggles[i].isOn) toggles[i].isOn = false;
        }
        // foreach (Toggle toggle in toggles)
        // {
        //     if (toggle.isOn)
        //     {
        //         toggle.isOn = false;
        //     }
        // }
        //button.interactable = false;

    }

}
