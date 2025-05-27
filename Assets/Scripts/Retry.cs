using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Retry : MonoBehaviour
{
    public TextMeshProUGUI RetryText;
    IEnumerator coroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RetryButton()
    {
        SceneManager.LoadScene("Prison");
    }

    public void Button_In()
    {
        coroutine = BlinkText();
        StartCoroutine(coroutine);
    }

    public void Button_Out()
    {
        RetryText.enabled = true;
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            RetryText.enabled = true;
            yield return new WaitForSeconds(0.1f);
            RetryText.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
