namespace InternetEmpire
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ClientManager : MonoBehaviour
    {
        public GameObject clientPrefab;
        public DeviceManager deviceManager;

        public ClientList clientList;

        public List<ClientController> clients = new List<ClientController>();

        public void GenerateClients( Client client, Device device, Vector3 spawnPosition)
        {
            GameObject clientObject = CreateClient(client, spawnPosition);
            DeviceController deviceController = CreateDevice(device.deviceType, spawnPosition);
            AssignDeviceToClient(clientObject, deviceController);
        }

        private GameObject CreateClient(Client client, Vector3 spawnPosition)
        {
            GameObject clientObject = Instantiate(clientPrefab, spawnPosition, Quaternion.identity);
            ClientController clientController = clientObject.GetComponent<ClientController>();
            clientController.ClientData = client;
            clients.Add(clientController);
            return clientObject;
        }

        private DeviceController CreateDevice(DeviceList.DeviceType deviceType, Vector3 spawnPosition)
        {
            return deviceManager.GenerateDevices(deviceType, spawnPosition);
        }

        private void AssignDeviceToClient(GameObject client, DeviceController device)
        {
            ClientController clientController = client.GetComponent<ClientController>();
            clientController.ClientData.Device = device.DeviceData;
            //set the device controller to the client controller child
            device.transform.SetParent(client.transform);
        }
    }
}