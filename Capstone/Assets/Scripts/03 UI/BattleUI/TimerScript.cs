using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    private float timerDuration;
    private TextMeshProUGUI timerText; // UI에 표시할 타이머 텍스트

    private float currentTime;
    private bool isTimerRunning = false;

    private void Start()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                OnTimerEnd();
            }
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        this.gameObject.SetActive(true);

        timerDuration = Random.Range(60, 91);  // 1분부터 1분 30초까지 랜덤

        currentTime = timerDuration;
        isTimerRunning = true;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60);
                int seconds = Mathf.FloorToInt(currentTime % 60);
                timerText.text = string.Format("남은 시간  {0:00} : {1:00}", minutes, seconds); // 분:초 형식으로 표시
            }
        }
    }

    private void OnTimerEnd()
    {
        this.gameObject.SetActive(false);
        // 플레이어 죽음
    }
}
