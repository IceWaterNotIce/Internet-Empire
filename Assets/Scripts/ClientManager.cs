using UnityEngine;

public class Client : MonoBehaviour
{
    public string clientName;
    public ClientManager.ClientType clientType;

    public Device device;
}

public class ClientManager : MonoBehaviour
{
    public enum ClientType { Home, Office, School, Hospital }

    public DeviceManager deviceManager;

    public void GenerateClients(string clientName, ClientType clientType, DeviceManager.DeviceType deviceType, Vector3 spawnPosition)
    {
        // 生成 empty gameobject 作為客戶
        GameObject client = new GameObject(clientName);
        client.transform.position = spawnPosition;
        client.AddComponent<Client>().clientName = clientName;
        client.GetComponent<Client>().clientType = clientType;
        Device device = deviceManager.GenerateDevices($"{clientName}'s {deviceType}", deviceType, spawnPosition);
        client.GetComponent<Client>().device = device;
        // 將裝置設為客戶的子物件
        device.transform.parent = client.transform;
    }
}