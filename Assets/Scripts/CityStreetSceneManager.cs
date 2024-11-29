using System.Collections.Generic;
using UnityEngine;

public class CityStreet : MonoBehaviour
{
    public DeviceManager deviceManager;
    public ClientManager clientManager;

    public int maxClients = 10; // 最多客戶數量
    public float spawnAreaWidth = 10f; // 生成區域寬度
    public float spawnAreaHeight = 10f; // 生成區域高度
    public float minSpawnTime = 3f; // 最小生成時間（秒）
    public float maxSpawnTime = 5f; // 最大生成時間（秒）
    public float spawnTimeIncrease = 0.3f; // 每次生成後增加的時間
    public float maxSpawnTimeLimit = 10f; // 最大生成等待時間（秒）

    void Start()
    {
        GenerateClients();
    }

    void GenerateClients()
    {
        for (int i = 0; i < maxClients; i++)
        {
            // 隨機生成位置
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
                0, // 假設在地面上生成，Y 軸為 0
                Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2)
            );

            // 隨機生成device
            DeviceManager.DeviceType deviceType = (DeviceManager.DeviceType)Random.Range(0, 4);
            // 生成客戶 
            ClientManager.ClientType clientType = (ClientManager.ClientType)Random.Range(0, 4);
            clientManager.GenerateClients($"Client {i + 1}", clientType, deviceType, spawnPosition);
        }
    }
}

