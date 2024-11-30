using UnityEngine;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour
{

    public DeviceManager deviceManager;

    public List<Client> clients = new List<Client>();

    public void GenerateClients(string clientName, ClientList.ClientType clientType, DeviceList.DeviceType deviceType, Vector3 spawnPosition)
    {
        GameObject client = CreateClient(clientName, clientType, spawnPosition);
        DeviceController device = CreateDevice(deviceType, spawnPosition);
        AssignDeviceToClient(client, device);
    }

    private GameObject CreateClient(string clientName, ClientList.ClientType clientType, Vector3 spawnPosition)
    {
        GameObject client = new GameObject(clientName);
        client.transform.position = spawnPosition;
        Client clientComponent = client.AddComponent<Client>();
        clientComponent.Initialize(clientName, clientType, null);
        clients.Add(clientComponent);
        return client;
    }

    private DeviceController CreateDevice(DeviceList.DeviceType deviceType, Vector3 spawnPosition)
    {
        return deviceManager.GenerateDevices(deviceType, spawnPosition);
    }

    private void AssignDeviceToClient(GameObject client, DeviceController device)
    {
        Client clientComponent = client.GetComponent<Client>();
        clientComponent.device = device.DeviceData;
        device.transform.parent = client.transform;
    }
}