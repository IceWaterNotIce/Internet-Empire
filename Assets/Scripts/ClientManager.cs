using UnityEngine;
using System.Collections.Generic;

public class ClientManager : MonoBehaviour
{
    public enum ClientType { Home, Office, School, Hospital }

    public DeviceManager deviceManager;

    public List<Client> clients = new List<Client>();

    public void GenerateClients(string clientName, ClientType clientType, DeviceManager.DeviceType deviceType, Vector3 spawnPosition)
    {
        GameObject client = CreateClient(clientName, clientType, spawnPosition);
        Device device = CreateDevice(clientName, deviceType, spawnPosition);
        AssignDeviceToClient(client, device);
    }

    private GameObject CreateClient(string clientName, ClientType clientType, Vector3 spawnPosition)
    {
        GameObject client = new GameObject(clientName);
        client.transform.position = spawnPosition;
        Client clientComponent = client.AddComponent<Client>();
        clientComponent.Initialize(clientName, clientType, null);
        return client;
    }

    private Device CreateDevice(string clientName, DeviceManager.DeviceType deviceType, Vector3 spawnPosition)
    {
        return deviceManager.GenerateDevices($"{clientName}'s {deviceType}", deviceType, spawnPosition);
    }

    private void AssignDeviceToClient(GameObject client, Device device)
    {
        Client clientComponent = client.GetComponent<Client>();
        clientComponent.device = device;
        device.transform.parent = client.transform;
    }
}