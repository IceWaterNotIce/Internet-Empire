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
    private Vector3 lastMousePosition;
    private Camera mainCamera;
    private Vector3 targetCameraPosition;

    public TMP_Text tmpGameTime; // Reference to the UI Text element for game time
    private DateTime gameTime; // The game time

    public float money; // The player's money
    public TMP_Text tmpMoney; // Reference to the UI Text element for money

    public float initialRadius; // 初始半徑
    public float radiusIncreaseRate; // 半徑增加速率
    public float maxRadius; // 最大半徑
    private float currentRadius; // 當前半徑

    void Start()
    {
        mainCamera = Camera.main;
        targetCameraPosition = mainCamera.transform.position;
        StartCoroutine(GenerateClients());
        gameTime = DateTime.Now; // Initialize game time with the current time
        currentRadius = initialRadius; // Initialize the current radius
    }

    void Update()
    {
        HandleCameraMovement();
        SmoothCameraMovement();
        UpdateGameTime();
        UpdateMoneyText();
        UpdateRadius();
    }

    void HandleCameraMovement()
    {
        if (Input.GetMouseButtonDown(2)) // 按下滑鼠滾輪
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) // 拖動滑鼠滾輪
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x, 0, -delta.y) * 10f;
            targetCameraPosition += move * Time.unscaledDeltaTime;
            lastMousePosition = Input.mousePosition;
        }
    }

    void SmoothCameraMovement()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.unscaledDeltaTime * 5f);
    }

    void UpdateRadius()
    {
        currentRadius = Mathf.Min(currentRadius + radiusIncreaseRate * Time.deltaTime, maxRadius);
    }

    IEnumerator GenerateClients()
    {
        while (currentClientCount < maxClients)
        {
            Vector3 spawnPosition;
            bool positionIsValid;

            do
            {
                spawnPosition = GetRandomSpawnPosition();

                positionIsValid = true;

                // 檢查生成點附近是否有其他客戶
                foreach (Client client in clientManager.clients)
                {
                    if (Vector3.Distance(spawnPosition, client.transform.position) < minDistanceBetweenClients)
                    {
                        positionIsValid = false;
                        break;
                    }
                }
            } while (!positionIsValid);

            // 隨機生成device
            DeviceManager.DeviceType deviceType = (DeviceManager.DeviceType)UnityEngine.Random.Range(0, 4);
            // 生成客戶 
            ClientManager.ClientType clientType = (ClientManager.ClientType)UnityEngine.Random.Range(0, 4);
            clientManager.GenerateClients($"Client {currentClientCount + 1}", clientType, deviceType, spawnPosition);

            currentClientCount++;

            // 等待隨機時間後生成下一個客戶
            float waitTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // 增加生成等待時間範圍
            minSpawnTime = Mathf.Min(minSpawnTime + spawnTimeIncrease, maxSpawnTimeLimit);
            maxSpawnTime = Mathf.Min(maxSpawnTime + spawnTimeIncrease, maxSpawnTimeLimit);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        float radius = UnityEngine.Random.Range(0f, currentRadius);
        return new Vector3(
            Mathf.Cos(angle) * radius,
            1, // 假設在地面上生成，Y 軸為 1
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
