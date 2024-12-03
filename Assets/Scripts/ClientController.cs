using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace InternetEmpire
{


    public class ClientController : MonoBehaviour
    {
        private ClientType clientData;

        public ClientType ClientData
        {
            get { return clientData; }
            set { clientData = value; }
        }

        private DeviceController deviceController;

        private int satisfaction;
        public int Satisfaction
        {
            get { return satisfaction; }
            set
            {
                if (value <= 0)
                {
                    satisfaction = 0;
                    Destroy(gameObject);
                    return;
                }
                else if (value > 100)
                {
                    Debug.LogError("Satisfaction cannot be greater than 100.");
                    satisfaction = 100;
                }
                satisfaction = value;
                UpdateUI();
            }
        }


        [SerializeField] private Slider satisfactionSlider;

        void Start()
        {
            if (clientData.demandGenerationTime > 0)
            { StartCoroutine(GenerateDemand()); }
            // get the device controller from child device
            deviceController = GetComponentInChildren<DeviceController>();
            deviceController.ConnectionsCount = Random.Range(0, deviceController.DeviceData.maxConnections - 1); // at least 1 connection is needed
            satisfaction = clientData.satisfaction;

            satisfactionSlider.maxValue = satisfaction;
            UpdateUI();

        }

        void Update()
        {

        }

        IEnumerator GenerateDemand()
        {
            while (true)
            {
                yield return new WaitForSeconds(clientData.demandGenerationTime);
                float demand = Random.Range(clientData.minDemand, clientData.maxDemand);
                Debug.Log("Demand: " + demand);
                CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
                ClientManager clientManager = FindFirstObjectByType<ClientManager>();
                // randomly select a client but not the current client
                ClientController client = clientManager.clients[Random.Range(0, clientManager.clients.Count)];
                if (client == this)
                {
                    yield break;
                }
                // check the current client can connect to the selected client
                ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
                Debug.Log(deviceController);
                Debug.Log(client.deviceController);
                Debug.Log(deviceController == client.deviceController);
                Debug.Log("Connecte " + connectionManager.connections.Count);
                Debug.Log("Connecte " + connectionManager.CanConnect(connectionManager.connections, deviceController, client.deviceController));
                if (connectionManager.CanConnect(connectionManager.connections, deviceController, client.deviceController))
                {

                    cityStreetSceneManager.money += demand;
                    Satisfaction += 1;
                }
                else
                {
                    //Todo: decrease the satisfaction of the client
                    Satisfaction -= 25;
                    Debug.Log("Satisfaction: " + Satisfaction);
                }
            }
        }

        void UpdateUI()
        {
            satisfactionSlider.value = satisfaction;
        }
    }
}