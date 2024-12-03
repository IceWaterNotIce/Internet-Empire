namespace InternetEmpire
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ClientManager : MonoBehaviour
    {
        public GameObject clientPrefab;
        public DeviceManager deviceManager;

        public ClientTypeList clientList;

        public List<ClientDevice> clients = new List<ClientDevice>();

        public void GenerateClients( ClientType client, global::DeviceType device, Vector3 spawnPosition)
        {
            GameObject clientObject = CreateClient(client, spawnPosition);
            DeviceController deviceController = CreateDevice(device.deviceType, spawnPosition);
            AssignDeviceToClient(clientObject, deviceController);
        }

        private GameObject CreateClient(ClientType client, Vector3 spawnPosition)
        {
            GameObject clientObject = Instantiate(clientPrefab, spawnPosition, Quaternion.identity);
            ClientDevice clientController = clientObject.GetComponent<ClientDevice>();
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
            ClientDevice clientController = client.GetComponent<ClientDevice>();
            clientController.ClientData.Device = device.DeviceData;
            //set the device controller to the client controller child
            device.transform.SetParent(client.transform);
        }
    }
}