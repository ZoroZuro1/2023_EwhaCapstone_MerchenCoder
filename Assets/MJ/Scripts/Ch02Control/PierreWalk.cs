using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierreWalk : MonoBehaviour
{
    bool isWalking = false;
    float targetX = 68.39f; // 목표 X 좌표
    float speed = 5f; // 이동 속도

    void Update()
    {
        if (GameManager.Instance.CheckPlayProgress("Mission2-7Y") && !isWalking)
        {
            // 조건이 충족되고 현재 움직이고 있지 않은 상태일 때
            isWalking = true; // 움직이는 중임을 표시
        }
        else
        {
            isWalking = false;
        }

        if (isWalking)
        {
            Walking();
        }
    }

    void Walking()
    {
        // 목표 지점으로 이동
        float step = speed * Time.deltaTime; // 프레임 간 이동량 계산
        transform.localEulerAngles = new Vector3(0, 180, 0);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(targetX, -1.63f, 0), step);

        // 목표 위치에 도달하면 isWalking을 false로 설정
        if (Mathf.Approximately(transform.localPosition.x, targetX))
        {
            isWalking = false;
            this.transform.localPosition = new Vector3(targetX, -1.72f, 0f);
            transform.localEulerAngles = new Vector3(0, 0, 0);
            this.gameObject.SetActive(false);
        }
    }
}
