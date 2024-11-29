using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public GameObject messagePanel; // Reference to the UI Panel containing the Text element
    public TMP_Text messageText; // Reference to the UI Text element

    void Start()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false); // 禁用 UI 物件
        }
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messagePanel != null && messageText != null)
        {
            messagePanel.SetActive(true); // 激活 UI 物件
            messageText.text = message;
            StartCoroutine(ClearMessageAfterDelay(duration));
        }
    }

    private IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (messagePanel != null)
        {
            messagePanel.SetActive(false); // 禁用 UI 物件
        }
    }
}