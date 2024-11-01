using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Button stage1Button;
    public Button stage2Button;
    public Button stage3Button;
    public Button stage4Button;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitializeStages(); // 초기화
        CheckStageUnlocks(); // 버튼 활성화 상태 설정
    }

    // 초기 설정: 처음 실행 시 Stage 1만 활성화
    void InitializeStages()
    {
        if (!PlayerPrefs.HasKey("Stage1Unlocked"))
        {
            PlayerPrefs.SetInt("Stage1Unlocked", 1); // Stage 1 활성화
            PlayerPrefs.SetInt("Stage2Unlocked", 0); // Stage 2~4 비활성화
            PlayerPrefs.SetInt("Stage3Unlocked", 0);
            PlayerPrefs.SetInt("Stage4Unlocked", 0);
        }
    }

    // 각 스테이지 버튼의 활성화 상태를 확인하고 설정
    void CheckStageUnlocks()
    {
        if (stage1Button != null)
            stage1Button.interactable = PlayerPrefs.GetInt("Stage1Unlocked") == 1;

        if (stage2Button != null)
            stage2Button.interactable = PlayerPrefs.GetInt("Stage2Unlocked") == 1;

        if (stage3Button != null)
            stage3Button.interactable = PlayerPrefs.GetInt("Stage3Unlocked") == 1;

        if (stage4Button != null)
            stage4Button.interactable = PlayerPrefs.GetInt("Stage4Unlocked") == 1;
    }

    // 현재 스테이지를 클리어하면 다음 스테이지 잠금 해제
    public void UnlockNextStage(int currentStage)
    {
        switch (currentStage)
        {
            case 1:
                PlayerPrefs.SetInt("Stage2Unlocked", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("Stage3Unlocked", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("Stage4Unlocked", 1);
                break;
        }

        PlayerPrefs.Save(); // 변경사항 저장
        CheckStageUnlocks(); // 업데이트 후 버튼 상태 재설정
    }
}
