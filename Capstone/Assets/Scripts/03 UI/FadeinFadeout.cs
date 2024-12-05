using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeinFadeout : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField]
    private float fadeDuration = 1.0f;

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        Color color = fadeImage.color;
        color.a = 1.0f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0.0f;
        fadeImage.color = color;
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        Color color = fadeImage.color;
        color.a = 0.0f;
        fadeImage.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1.0f;
        fadeImage.color = color;
    }

    IEnumerator FadeBattleUI()
    {
        gameObject.SetActive(true); // 오브젝트를 활성화하고 페이드 인 진행
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(FadeOut());
        gameObject.SetActive(false); // 페이드 아웃 후 오브젝트를 다시 비활성화
    }

    public void ShowBattleUI()
    {
      
    }

    private void Start()
    {
       
    }
}
