using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace InternetEmpire
{
    public class MessageManager : Singleton<MessageManager>
    {

        private GameObject panelprefab;
        private GameObject tmpTextPrefab;
        private GameObject buttonPrefab;
        private GameObject buttonClosePrefab;
        void Start()
        {
            panelprefab = Resources.Load<GameObject>("Prefabs/Messages/Panel");
            tmpTextPrefab = Resources.Load<GameObject>("Prefabs/Messages/TMP_Text");
            buttonPrefab = Resources.Load<GameObject>("Prefabs/Messages/Button");
            buttonClosePrefab = Resources.Load<GameObject>("Prefabs/Messages/ButtonClose");
        }

        public void CreateYesNoMessage(string message, System.Action onYes, System.Action onNo)
        {
            GameObject panel = Instantiate(panelprefab, transform);
            GameObject tmpText = Instantiate(tmpTextPrefab, panel.transform);
            tmpText.GetComponent<TMP_Text>().text = message;
            GameObject buttonYes = Instantiate(buttonPrefab, panel.transform);
            buttonYes.GetComponentInChildren<TMP_Text>().text = "Yes";
            buttonYes.GetComponent<Button>().onClick.AddListener(() =>
            {
                onYes?.Invoke();
                Destroy(panel);
            });
            GameObject buttonNo = Instantiate(buttonPrefab, panel.transform);
            buttonNo.GetComponentInChildren<TMP_Text>().text = "No";
            buttonNo.GetComponent<Button>().onClick.AddListener(() =>
            {
                onNo?.Invoke();
                Destroy(panel);
            });
        }

        public void ToastMessage(string message, float duration = 2f)
        {
            GameObject panel = Instantiate(panelprefab, transform);
            GameObject tmpText = Instantiate(tmpTextPrefab, panel.transform);
            tmpText.GetComponent<TMP_Text>().text = message;
            StartCoroutine(DestroyAfter(panel, duration));
        }

        private IEnumerator DestroyAfter(GameObject panel, float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(panel);
        }

        public void CreateCloseMessage(string message, System.Action onClose)
        {
            GameObject panel = Instantiate(panelprefab, transform);
            GameObject tmpText = Instantiate(tmpTextPrefab, panel.transform);
            tmpText.GetComponent<TMP_Text>().text = message;
            GameObject buttonClose = Instantiate(buttonClosePrefab, panel.transform);
            buttonClose.GetComponentInChildren<TMP_Text>().text = "Close";
            buttonClose.GetComponent<Button>().onClick.AddListener(() =>
            {
                onClose?.Invoke();
                Destroy(panel);
            });
        }
    }
}
