namespace InternetEmpire
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using TMPro;
    using System.Linq;

    public class CityStreetSceneManager : MonoBehaviour
    {
        public DeviceManager deviceManager;
        public ClientManager clientManager;

        public int maxClients; // 最多客戶數量
        public float minSpawnTime; // 最小生成時間（秒）
        public float maxSpawnTime; // 最大生成時間（秒）
        public float spawnTimeIncrease; // 每次生成後增加的時間
        public float maxSpawnTimeLimit; // 最大生成等待時間（秒）
        public float minDistanceBetweenClients; // 客戶之間的最小距離

        private int currentClientCount; // 當前客戶數量
        public TMP_Text tmpGameTime; // Reference to the UI Text element for game time
        public DateTime gameTime; // The game time

        public float money; // The player's money
        public TMP_Text tmpMoney; // Reference to the UI Text element for money

        public float initialRadius; // 初始半徑
        public float radiusIncreaseRate; // 半徑增加速率
        public float maxRadius; // 最大半徑
        public float currentRadius; // 當前半徑

        public DeviceTypeList deviceList;

        public GameObject ConnectionPanel;

        public GameObject DevicePanel;

        public ConnectionManager connectionManager;

        public void ToggleConnectionPanel()
        {
            ConnectionPanel.SetActive(!ConnectionPanel.activeSelf);
            connectionManager.currentMethod = null;
        }

        public void ToggleDevicePanel()
        {
            DevicePanel.SetActive(!DevicePanel.activeSelf);
            deviceManager.PlayerDevice = null;
        }

        void Start()
        {
            StartCoroutine(GenerateClients());
            gameTime = DateTime.Now; // Initialize game time with the current time
            currentRadius = initialRadius; // Initialize the current radius
            // LoadGame();

        }

        void Update()
        {
            UpdateGameTime();
            UpdateMoneyText();
            UpdateRadius();
        }

        void UpdateRadius()
        {
            currentRadius = Mathf.Min(currentRadius + radiusIncreaseRate * Time.deltaTime, maxRadius);
        }

        IEnumerator GenerateClients()
        {
            while (currentClientCount < maxClients)
            {
                Vector2 spawnPosition;
                bool positionIsValid;
                int maxAttempts = 5; // 最大嘗試次數
                int attempts = 0;
                do
                {
                    spawnPosition = GetRandomSpawnPosition();
                    positionIsValid = true;

                    // 檢查生成點附近是否有其他客戶
                    foreach (Client existingClient in clientManager.clients)
                    {
                        foreach (ClientDevice existingClientDevice in existingClient.Devices)
                        {
                            // 檢查物件是否存在
                            if (existingClientDevice == null)
                            {
                                continue;
                            }
                            if (Vector2.Distance(existingClientDevice.transform.position, spawnPosition) < minDistanceBetweenClients)
                            {
                                positionIsValid = false;
                                break;
                            }
                        }
                    }


                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        Debug.LogWarning("Failed to find a valid spawn position after maximum attempts.");

                        yield return new WaitForSeconds(5);
                    }
                } while (!positionIsValid);

                // 隨機getting device
                if (deviceList == null)
                {
                    Debug.LogError("DeviceList is not assigned.");
                    yield break;
                }

                DeviceModel device = deviceList.GetDevice(UnityEngine.Random.Range(0, deviceList.GetCount()));
                if (device == null)
                {
                    Debug.LogError("Failed to get a device from DeviceList.");
                    yield break;
                }

                ClientType client = clientManager.clientList.GetClient(UnityEngine.Random.Range(0, clientManager.clientList.GetCount()));
                if (client == null)
                {
                    Debug.LogError("Failed to get a client from ClientList.");
                    yield break;
                }

                clientManager.GenerateClients(client, device, spawnPosition);



                currentClientCount++;

                // 等待隨機時間後生成下一個客戶
                float waitTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(waitTime);

                // 增加生成等待時間範圍
                minSpawnTime = Mathf.Min(minSpawnTime + spawnTimeIncrease, maxSpawnTimeLimit);
                maxSpawnTime = Mathf.Min(maxSpawnTime + spawnTimeIncrease, maxSpawnTimeLimit);
            }
        }

        Vector2 GetRandomSpawnPosition()
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
            float radius = UnityEngine.Random.Range(0f, currentRadius);
            return new Vector2(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius
            );
        }

        public void SpeedUp(float speed)
        {
            Time.timeScale = speed; // 將遊戲速度加倍
        }

        public void Pause()
        {
            Time.timeScale = 0f; // 暫停遊戲
        }

        public void Resume()
        {
            Time.timeScale = 1f; // 恢復正常速度
        }

        void UpdateGameTime()
        {
            gameTime = gameTime.AddSeconds(Time.deltaTime * 60); // 1秒遊戲時間 = 1分鐘現實時間
            UpdateGameTimeText();
        }

        void UpdateGameTimeText()
        {
            if (tmpGameTime != null)
            {
                tmpGameTime.text = gameTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        void UpdateMoneyText()
        {
            if (tmpMoney != null)
            {
                tmpMoney.text = $"${money}";
            }
        }

        public void GoToLobby()
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }

        public GameObject SettingPanelObject;

        public void ToggleSettingPanel()
        {
            SettingPanelObject.SetActive(!SettingPanelObject.activeSelf);
        }

        // public void SaveGame()
        // {
        //     List<GameData> clientData = new List<GameData>();
        //     foreach (Client client in clientManager.clients)
        //     {
        //         foreach (ClientDevice clientDevice in client.Devices)
        //         {
        //             GameData data = new GameData();
        //             data.position = clientDevice.transform.position;
        //             data.rotation = clientDevice.transform.rotation;
        //             data.objType = "ClientDevice";
        //             data.clientType = client.Type.name;
        //             data.deviceModel = clientDevice.Device.Model.name;
        //             data.satisfaction = client.Satisfaction;
        //             clientData.Add(data);
        //         }
        //     }

        //     GameDataList clientDataList = new GameDataList(clientData);
        //     string clientsJson = JsonUtility.ToJson(clientDataList);
        //     PlayerPrefs.SetString("clientData", clientsJson);

        //     List<GameData> deviceData = new List<GameData>();
        //     foreach (Device device in deviceManager.devices)
        //     {
        //         GameData data = new GameData();
        //         data.position = device.transform.position;
        //         data.rotation = device.transform.rotation;
        //         data.objType = "Device";
        //         data.deviceModel = device.Model.name;
        //         deviceData.Add(data);
        //     }

        //     GameDataList deviceDataList = new GameDataList(deviceData);
        //     string devicesJson = JsonUtility.ToJson(deviceDataList);
        //     PlayerPrefs.SetString("deviceData", devicesJson);

        //     List<GameData> connectionData = new List<GameData>();
        //     foreach (Connection connection in connectionManager.connections)
        //     {
        //         GameData data = new GameData();
        //         data.position = connection.transform.position;
        //         data.rotation = connection.transform.rotation;
        //         data.objType = "Connection";
        //         data.clientType = connection.ConnectionData.name;
        //         connectionData.Add(data);
        //     }

        //     GameDataList connectionDataList = new GameDataList(connectionData);
        //     string connectionsJson = JsonUtility.ToJson(connectionDataList);
        //     PlayerPrefs.SetString("connectionData", connectionsJson);

        //     PlayerPrefs.Save();
        //     Debug.Log("Game Saved");
        // }

        // public void LoadGame()
        // {
        //     string clientsJson = PlayerPrefs.GetString("clientData");
        //     GameDataList clientDataList = JsonUtility.FromJson<GameDataList>(clientsJson);
        //     List<GameData> clientData = clientDataList.datas;

        //     foreach (GameData data in clientData)
        //     {
        //         if (data.objType == "ClientDevice")
        //         {
        //             ClientType clientType = clientManager.clientList.clients.FirstOrDefault(c => c.name == data.clientType);
        //             DeviceModel deviceModel = deviceManager.deviceList.devices.FirstOrDefault(d => d.name == data.deviceModel);
        //             if (clientType != null && deviceModel != null)
        //             {
        //                 clientManager.GenerateClients(clientType, deviceModel, data.position);
        //                 Client client = clientManager.clients.Last();
        //                 client.Satisfaction = data.satisfaction;
        //             }
        //         }
        //     }

        //     string devicesJson = PlayerPrefs.GetString("deviceData");
        //     GameDataList deviceDataList = JsonUtility.FromJson<GameDataList>(devicesJson);
        //     List<GameData> deviceData = deviceDataList.datas;

        //     foreach (GameData data in deviceData)
        //     {
        //         if (data.objType == "Device")
        //         {
        //             DeviceModel deviceModel = deviceManager.deviceList.devices.FirstOrDefault(d => d.name == data.deviceModel);
        //             if (deviceModel != null)
        //             {
        //                 GameObject deviceObject = Instantiate(deviceManager.devicePrefab, data.position, data.rotation);
        //                 Device device = deviceObject.GetComponent<Device>();
        //                 device.Model = deviceModel;
        //                 deviceManager.devices.Add(device);
        //             }
        //         }
        //     }

        //     string connectionsJson = PlayerPrefs.GetString("connectionData");
        //     GameDataList connectionDataList = JsonUtility.FromJson<GameDataList>(connectionsJson);
        //     List<GameData> connectionData = connectionDataList.datas;

        //     foreach (GameData data in connectionData)
        //     {
        //         if (data.objType == "Connection")
        //         {
        //             GameObject connectionObject = Instantiate(connectionManager.connectionPrefab, data.position, data.rotation);
        //             Connection connection = connectionObject.GetComponent<Connection>();
        //             connection.ConnectionData = connectionManager.connectionList.connections.FirstOrDefault(c => c.name == data.clientType);
        //             connectionManager.connections.Add(connection);
        //         }
        //     }

        //     Debug.Log("Game Loaded");



        //     // the step of loading the game
        //     // 1. Load the client and instantiate the client object
        //     // 2. Load the client device and instantiate the client device object
        //     // 3. Set the client device parent to its client
        //     // 4. Load the device and instantiate the device object
        //     // 5. Load the connection and instantiate the connection object to connect the devices
        //     // 6. Load the network packet and instantiate the network packet object to transfer data between devices
        //     // clientManager.LoadClients();
        //     // deviceManager.LoadDevices();
        //     // connectionManager.LoadConnections();
            

        // }
    }

    [System.Serializable]
    public class GameDataList
    {
        public List<GameData> datas;

        public GameDataList(List<GameData> datas)
        {
            this.datas = datas;
        }
    }
    public class GameData
    {
        public Vector3 position;
        public Quaternion rotation;
        public string objType;
        public string clientType;
        public string deviceModel;
        public int satisfaction;
    }
}
