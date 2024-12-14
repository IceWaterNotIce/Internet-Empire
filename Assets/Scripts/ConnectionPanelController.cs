using UnityEngine;
using UnityEngine.UI;

namespace InternetEmpire
{
    public class ConnectionPanelController : MonoBehaviour
    {
        [SerializeField] private ConnectionList connectionList;
        [SerializeField] private GameObject connectionButtonPrefab;
        

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
    }
}
