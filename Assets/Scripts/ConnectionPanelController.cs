using UnityEngine;
using UnityEngine.UI;

public class ConnectionPanelController : MonoBehaviour
{
    [SerializeField] private ConnectionList connectionList;
    [SerializeField] private GameObject connectionButtonPrefab;

    [SerializeField] private ConnectionManager connectionManager;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        foreach (Connection connection in connectionList.connections)
        {
            Sprite ConnectionPreviewSprite = connection.sprite;
            GameObject connectionButton = Instantiate(connectionButtonPrefab, transform);
            Image image = connectionButton.GetComponent<Image>();
            image.sprite = ConnectionPreviewSprite;

            // Add a button click listener
            Button button = connectionButton.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                connectionManager.SetConnectionMethod(connection);
            });
        }
    }
}
