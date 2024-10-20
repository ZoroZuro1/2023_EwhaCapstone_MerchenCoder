using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class GameLoadingScript : MonoBehaviour
{
    public float loadValue;
    public Text loadText;
    public Slider loadingProgress;


    // Start is called before the first frame update
    void Start()
    {
        loadValue = 0.0f;
    }


    IEnumerator UpdateProgress(float value, float time = 0.01f)
    {
        float elaspedTime = 0f;
        float startValue = loadingProgress.value;
        float targetValue = value;

        while (elaspedTime < time)
        {
            elaspedTime += Time.deltaTime;
            loadingProgress.value = Mathf.Lerp(startValue, targetValue, elaspedTime / time);
            loadText.text = loadValue.ToString();
            yield return null;
        }

        // 정확하게 목표값에 도달
        loadingProgress.value = targetValue;
        loadText.text = targetValue.ToString();
        print($"loadValue : {targetValue}");

    }
    public IEnumerator Loading()
    {
        if (Directory.Exists(Path.Combine(Application.persistentDataPath, "static")))
        {
            Debug.Log("이미 로컬에 s3에서 가져온 정적파일 존재");
            StartCoroutine(UpdateProgress(30, 0.4f));
            DataManager.Instance.GetComponent<FullLoad>().LoadAllData((success) =>
            {
                if (success)
                {
                    DataManager.Instance.LoadGameStatusData();
                    GameManager.Instance.LoadPlayerData();
                    GameManager.Instance.GetComponent<PlayData>().LoadPlayData();
                    UpdateProgress(50, 0.4f);
                    StartCoroutine(AsycLoadHomeScene());
                }
                else
                {
                    Debug.LogError("데이터 로드 실패. 실행 중지합니다.");
                    //UnityEditor.EditorApplication.isPlaying = false;
                    return;

                }
            });
        }
        else
        {
            DataManager.Instance.GetComponent<FullLoad>().LoadFromS3((success) =>
            {
                if (success)
                {

                    StartCoroutine(UpdateProgress(30, 0.4f));
                    DataManager.Instance.GetComponent<FullLoad>().LoadAllData((success) =>
                    {
                        if (success)
                        {
                            DataManager.Instance.LoadGameStatusData();
                            GameManager.Instance.LoadPlayerData();
                            GameManager.Instance.GetComponent<PlayData>().LoadPlayData();
                            UpdateProgress(50, 0.4f);
                            StartCoroutine(AsycLoadHomeScene());
                        }
                        else
                        {
                            Debug.LogError("데이터 로드 실패. 실행 중지합니다.");
                            //UnityEditor.EditorApplication.isPlaying = false;
                            return;

                        }
                    });
                }
                else
                {
                    Debug.LogError("s3 버킷에서 데이터 로드 실패. 실행을 중지합니다.");
                    // UnityEditor.EditorApplication.isPlaying = false;
                    return;
                }
            });
        }
        yield return null;

    }

    IEnumerator AsycLoadHomeScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Home");
        asyncLoad.allowSceneActivation = false;
        //isDone 은 asyncLoad.progress의 연산이 완료되었는지 확인한다.
        //allowSceneActivation을 true로 하면 씬 로드가 완료되면 바로 씬 전환이 되지만,
        //allowSceneActivation을 false로 하면 씬 로드 progress가 0.9에서 중지된다.
        //따라서 allowSceneActivation false를 쓰는 경우 while문 밖을 탈출하지 못한다.
        //이 경우 allowSceneActivation true로 만들 수 있는 방법을 추가해 주어야한다.
        while (!asyncLoad.isDone)
        {
            //Debug.Log(asyncLoad.progress);
            if (asyncLoad.progress < 0.9f)
            {
                Debug.Log("adsfafasfafaf");
                float targetValue = Mathf.Round(asyncLoad.progress * 50 * 100) / 100 + 50;
                Debug.Log(targetValue);
                StartCoroutine(UpdateProgress(targetValue));
            }
            else
            {
                StartCoroutine(UpdateProgress(100, 0.5f));
                Debug.Log("씬 준비 완료. 잠시 대기");
                yield return new WaitForSeconds(1.5f);
                asyncLoad.allowSceneActivation = true;
            }


        }
        yield return null; // 추가
    }
}
