namespace InternetEmpire
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using TMPro;

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
            LoadGame();

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

        public void SaveGame()
        {
            // save time and money currentRadius

            PlayerPrefs.SetFloat("money", money);
            PlayerPrefs.SetString("gameTime", gameTime.ToString());
            PlayerPrefs.SetFloat("currentRadius", currentRadius);
            // save all ClientDevice

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

            money = PlayerPrefs.GetFloat("money");
            gameTime = System.DateTime.Parse(PlayerPrefs.GetString("gameTime"));
            currentRadius = PlayerPrefs.GetFloat("currentRadius");

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
