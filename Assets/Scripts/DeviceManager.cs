using UnityEngine;
using System.Collections.Generic;


public class DeviceManager : MonoBehaviour
{
    public enum DeviceType { Computer, Router, Switch, Server }
    public GameObject[] devicePrefabs; // 裝置預製件

    public List<Device> devices = new List<Device>(); 

    void Start()
    {

    }

    public Device GenerateDevices(string deviceName, DeviceType deviceType, Vector3 spawnPosition)
    {
        GameObject devicePrefab = devicePrefabs[(int)deviceType];
        GameObject device = Instantiate(devicePrefab, spawnPosition, Quaternion.identity);
        device.GetComponent<Device>().deviceName = deviceName;
        device.GetComponent<Device>().deviceType = deviceType;
        devices.Add(device.GetComponent<Device>());
        return device.GetComponent<Device>();
    }
}