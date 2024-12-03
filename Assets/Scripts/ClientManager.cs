namespace InternetEmpire
{
    using UnityEngine;
    using System.Collections.Generic;


    public class ClientManager : MonoBehaviour
    {
        public GameObject clientPrefab;

        public GameObject clientDevicePrefab;
        public DeviceManager deviceManager;

        public ClientTypeList clientList;

        public List<Client> clients = new List<Client>();

        public void GenerateClients(ClientType clientType, DeviceModel model, Vector3 spawnPosition)
        {
            if (clientType == null)
            {
                Debug.LogError("ClientType is null");
                return;
            }
            if (model == null)
            {
                Debug.LogError("DeviceModel is null");
                return;
            }
            if (spawnPosition == null)
            {
                Debug.LogError("Spawn position is null");
                return;
            }
            GameObject clientObject = CreateClient(clientType, spawnPosition);
            Client client = clientObject.GetComponent<Client>();
            GameObject clientDeviceObject = CreateClientDevice(client, model, spawnPosition);
            AssignDeviceToClient(clientObject, clientDeviceObject);

        }

        private GameObject CreateClient(ClientType clientType, Vector3 spawnPosition)
        {
            GameObject clientObject = Instantiate(clientPrefab, spawnPosition, Quaternion.identity);
            Client client = clientObject.GetComponent<Client>();
            client.Type = clientType;
            clients.Add(client);
            return clientObject;
        }

        private GameObject CreateClientDevice(Client client, DeviceModel model, Vector3 spawnPosition)
        {
            if (client == null)
            {
                Debug.LogError("Client is null");
                return null;
            }
            if (model == null)
            {
                Debug.LogError("DeviceModel is null");
                return null;
            }
            if (spawnPosition == null)
            {
                Debug.LogError("Spawn position is null");
                return null;
            }
            GameObject clientDevice = Instantiate(clientDevicePrefab, spawnPosition, Quaternion.identity);
            ClientDevice clientDeviceComponent = clientDevice.GetComponent<ClientDevice>();
            GameObject device = deviceManager.GenerateDevices(model, spawnPosition);
            Device deviceComponent = device.GetComponent<Device>();
            Debug.Log(client.Devices);
            Debug.Log(clientDeviceComponent);
            Debug.Log(deviceComponent);
            deviceComponent.transform.SetParent(clientDevice.transform);
            clientDeviceComponent.Device = deviceComponent;
            clientDeviceComponent.Client = client;
            client.Devices.Add(clientDeviceComponent);
            
            return clientDevice;
        }

        private void AssignDeviceToClient(GameObject client, GameObject clientDevice)
        {
            clientDevice.transform.SetParent(client.transform);
        }
    }
}