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
        private DateTime gameTime; // The game time

        public float money; // The player's money
        public TMP_Text tmpMoney; // Reference to the UI Text element for money

        public float initialRadius; // 初始半徑
        public float radiusIncreaseRate; // 半徑增加速率
        public float maxRadius; // 最大半徑
        public float currentRadius; // 當前半徑

        public DeviceList deviceList;

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
                    foreach (ClientDevice existingClient in clientManager.clients)
                    {
                        // check the object exists
                        if (existingClient == null)
                        {
                            continue;
                        }
                        if (Vector2.Distance(existingClient.transform.position, spawnPosition) < minDistanceBetweenClients)
                        {
                            positionIsValid = false;
                            break;
                        }
                    }

                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        Debug.LogError("Failed to find a valid spawn position after maximum attempts.");
                        
                        yield return new WaitForSeconds(5);
                    }
                } while (!positionIsValid);

                // 隨機getting device
                if (deviceList == null)
                {
                    Debug.LogError("DeviceList is not assigned.");
                    yield break;
                }

                global::DeviceType device = deviceList.GetDevice(UnityEngine.Random.Range(0, deviceList.GetCount()));
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

        public void SpeedUp(int speed)
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
    }
}
