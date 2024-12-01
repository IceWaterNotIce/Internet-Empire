using UnityEngine;
using System.Collections;

namespace InternetEmpire
{


    public class ClientController : MonoBehaviour
    {
        private Client clientData;

        public Client ClientData
        {
            get { return clientData; }
            set { clientData = value; }
        }

        private DeviceController deviceController;


        void Start()
        {
            if (clientData.demandGenerationTime > 0)
            { StartCoroutine(GenerateDemand()); }
            // get the device controller from child device
            deviceController = GetComponentInChildren<DeviceController>();
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
                while (client == this)
                {
                    client = clientManager.clients[Random.Range(0, clientManager.clients.Count)];
                }
                // check the current client can connect to the selected client
                ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
                if (connectionManager.CanConnect(connectionManager.connections, deviceController, client.deviceController))
                {
                    Debug.Log(deviceController);
                    Debug.Log(client.deviceController);
                    Debug.Log(deviceController == client.deviceController);
                    Debug.Log("Connecte " + connectionManager.connections.Count);
                    Debug.Log("Connecte " + connectionManager.CanConnect(connectionManager.connections, deviceController, client.deviceController));
                    cityStreetSceneManager.money += demand;
                }
                else
                {
                    //Todo: decrease the satisfaction of the client
                }
            }
        }
    }
}