using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(GenerateClients());
    }

    IEnumerator GenerateClients()
    {
        while (currentClientCount < maxClients)
        {
            Vector3 spawnPosition;
            bool positionIsValid;

            // 嘗試找到一個有效的生成位置
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
            DeviceManager.DeviceType deviceType = (DeviceManager.DeviceType)Random.Range(0, 4);
            // 生成客戶 
            ClientManager.ClientType clientType = (ClientManager.ClientType)Random.Range(0, 4);
            clientManager.GenerateClients($"Client {currentClientCount + 1}", clientType, deviceType, spawnPosition);

            currentClientCount++;

            // 等待隨機時間後生成下一個客戶
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // 增加生成等待時間範圍
            minSpawnTime = Mathf.Min(minSpawnTime + spawnTimeIncrease, maxSpawnTimeLimit);
            maxSpawnTime = Mathf.Min(maxSpawnTime + spawnTimeIncrease, maxSpawnTimeLimit);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float spawnAreaWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;
        float spawnAreaHeight = mainCamera.orthographicSize * 2;

        return new Vector3(
            Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
            0, // 假設在地面上生成，Y 軸為 0
            Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2)
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
}
