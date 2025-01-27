using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace InternetEmpire
{
    public class ConnectionPanelController : MonoBehaviour
    {
        [SerializeField] private ConnectionList connectionList;
        [SerializeField] private GameObject connectionButtonPrefab;

        [SerializeField] private Button connectionCancelButton;
        

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            foreach (ConnectionMethod connection in connectionList.connections)
            {
                GameObject connectionButton = Instantiate(connectionButtonPrefab, transform);
                connectionButton.GetComponent<ConnectionPanelConnectionMethodButton>().Connection = connection;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnDisable()
        {
            ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
            connectionManager.currentMethod = null;
            connectionManager.cancelConnection();
        }

        public void toggleConnectionCancelButton(bool bol)
        {
            connectionCancelButton.gameObject.SetActive(bol);
        }
    }
}
