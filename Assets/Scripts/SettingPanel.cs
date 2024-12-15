using UnityEngine;
using System.Collections.Generic;
using Unity.Services.CloudSave.Models.Data.Player;

namespace InternetEmpire
{

    public class SettingPanel : MonoBehaviour
    {
        public GameObject SettingPanelObject;

        public void ToggleSettingPanel()
        {
            SettingPanelObject.SetActive(!SettingPanelObject.activeSelf);
        }

        public void SaveGame()
        {
            // save time and money currentRadius
            CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
            PlayerPrefs.SetFloat("money", cityStreetSceneManager.money);
            PlayerPrefs.SetString("gameTime", cityStreetSceneManager.gameTime.ToString());
            PlayerPrefs.SetFloat("currentRadius", cityStreetSceneManager.currentRadius);

            // Save all the devices and clients
            DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();
            //Get all the devices
            List<Device> devices = deviceManager.devices;
            // save each device location
            List<GameData> deviceData = new List<GameData>();
            foreach (Device device in devices)
            {
                GameData data = new GameData();
                data.position = device.transform.position;
                data.rotation = device.transform.rotation;
                data.obj = device.Model;
                deviceData.Add(data);
            }
            Debug.Log(deviceData.Count);
            //Save all the devices to json
            GameDataList deviceDataList = new GameDataList(deviceData);
            string devicesJson = JsonUtility.ToJson(deviceDataList);

            Debug.Log(devicesJson);
            PlayerPrefs.SetString("deviceData", devicesJson);
            ClientManager clientManager = FindFirstObjectByType<ClientManager>();
            //Get all the clients
            List<Client> clients = clientManager.clients;
            List<GameData> clientData = new List<GameData>();
            foreach (Client client in clients)
            {
                GameData data = new GameData();
                data.position = client.transform.position;
                data.rotation = client.transform.rotation;
                data.obj = client.Type;
                clientData.Add(data);
            }
            //Save all the clients to json
            GameDataList clientDataList = new GameDataList(clientData);
            string clientsJson = JsonUtility.ToJson(clientDataList);
            PlayerPrefs.SetString("clientData", clientsJson);

            ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
            //Get all the connections
            List<Connection> connections = connectionManager.connections;
            List<GameData> connectionData = new List<GameData>();
            foreach (Connection connection in connections)
            {
                GameData data = new GameData();
                data.position = connection.transform.position;
                data.rotation = connection.transform.rotation;
                data.obj = connection;
                connectionData.Add(data);
            }
            //Save all the connections to json
            GameDataList connectionDataList = new GameDataList(connectionData);
            string connectionsJson = JsonUtility.ToJson(connectionDataList);
            PlayerPrefs.SetString("connectionData", connectionsJson);



            PlayerPrefs.Save();
            Debug.Log("Game Saved");

        }

        public void LoadGame()
        {

            // Load time and money
            CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
            cityStreetSceneManager.money = PlayerPrefs.GetFloat("money");
            cityStreetSceneManager.gameTime = System.DateTime.Parse(PlayerPrefs.GetString("gameTime"));
            cityStreetSceneManager.currentRadius = PlayerPrefs.GetFloat("currentRadius");

            // Load all the devices and clients
            DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();
            //Get all the devices
            Debug.Log(PlayerPrefs.GetString("deviceData"));
            GameDataList deviceDataList = JsonUtility.FromJson<GameDataList>(PlayerPrefs.GetString("deviceData"));
            List<GameData> deviceData = deviceDataList.datas;
            Debug.Log(deviceData.Count);
            List<Device> devices = new List<Device>();
            foreach (GameData data in deviceData)
            {
                GameObject deviceObject = deviceManager.GenerateDevices((DeviceModel)data.obj, data.position);
                Device device = deviceObject.GetComponent<Device>();
                device.transform.rotation = data.rotation;
                devices.Add(device);
                Debug.Log("Device Loaded");
            }
            deviceManager.devices = devices;

            ClientManager clientManager = FindFirstObjectByType<ClientManager>();
            //Get all the clients
            GameDataList clientDataList = JsonUtility.FromJson<GameDataList>(PlayerPrefs.GetString("clientData"));
            List<GameData> clientData = clientDataList.datas;
            List<Client> clients = new List<Client>();
            foreach (GameData data in clientData)
            {
                GameObject clientObject = Instantiate(clientManager.clientPrefab, data.position, data.rotation);
                Client client = clientObject.GetComponent<Client>();
                client.Type = (ClientType)data.obj;
                clients.Add(client);
            }
            clientManager.clients = clients;

            ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
            //Get all the connections
            GameDataList connectionDataList = JsonUtility.FromJson<GameDataList>(PlayerPrefs.GetString("connectionData"));
            List<GameData> connectionData = connectionDataList.datas;
            List<Connection> connections = new List<Connection>();
            foreach (GameData data in connectionData)
            {
                GameObject connectionObject = Instantiate(connectionManager.connectionPrefab, data.position, data.rotation);
                Connection connection = connectionObject.GetComponent<Connection>();
                connection.ConnectionData = (ConnectionMethod)data.obj;
                connections.Add(connection);
            }
            connectionManager.connections = connections;


            Debug.Log("Game Loaded");
        }
    }
    [System.Serializable]
    public class GameData
    {
        public Vector3 position;
        public Quaternion rotation;
        public object obj;
    }
    [System.Serializable]
public class GameDataList
{
    public List<GameData> datas;

    public GameDataList(List<GameData> datadtas)
    {
        this.datas = datadtas;
    }
}
}
