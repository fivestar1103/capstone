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

    [SerializeField]
    private GameObject explosionEffect; // 플레이어 폭발 이펙트

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

        timerDuration = Random.Range(3, 11);  // 1분부터 1분 30초까지 랜덤

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
        // 몬스터가 모두 퇴치되지 않은 채로 시간이 지나면 플레이어 사망
        // 정상적인 클리어는 구현 예정
        StartCoroutine(PlayerExplosion());
    }

    IEnumerator PlayerExplosion()
    {
        GameObject explosion = Instantiate(explosionEffect, PlayManager.PlayerPos, Quaternion.identity, PlayManager.PlayerTransform);
        yield return new WaitForSeconds(6.5f);  // 파티클 재생시간
        PlayManager.PlayerHit(PlayManager.MaxHP);
        Destroy(explosion);
    }
}
