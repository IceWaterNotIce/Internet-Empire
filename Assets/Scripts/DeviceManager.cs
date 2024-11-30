using UnityEngine;
using System.Collections.Generic;

public class DeviceManager : MonoBehaviour
{
    public GameObject devicePrefab; // 裝置預製件
    public DeviceList deviceList; // 參考 DeviceList

    public List<DeviceController> devices = new List<DeviceController>(); 

    void Start()
    {

    }

    public DeviceController GenerateDevices(DeviceList.DeviceType deviceType, Vector3 spawnPosition)
    {
        GameObject device = Instantiate(devicePrefab, spawnPosition, Quaternion.identity);
        DeviceController deviceController = device.GetComponent<DeviceController>();

        // 從 DeviceList 中獲取裝置資料
        Device deviceData = GetDeviceData(deviceType);

        deviceController.DeviceData = deviceData;
        devices.Add(deviceController);
        return deviceController;
    }

    private Device GetDeviceData(DeviceList.DeviceType deviceType)
    {
        foreach (Device device in deviceList.devices)
        {
            if (device.deviceType == deviceType)
            {
                return device;
            }
        }
        Debug.LogWarning($"Device with type {deviceType} not found in DeviceList.");
        return null;
    }
}