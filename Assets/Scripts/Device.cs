using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDevice", menuName = "Device")]
public class Device : ScriptableObject
{
    public string deviceName;
    public DeviceList.DeviceType deviceType;

    public List<Device> connectedDevices = new List<Device>();

    public int maxConnections = 1;



    public Sprite sprite;

    public void Connect(Device device)
    {
        if (!connectedDevices.Contains(device))
        {
            connectedDevices.Add(device);
            Debug.Log($"{deviceName} connected to {device.deviceName}.");
        }
        else
        {
            Debug.LogWarning($"{deviceName} is already connected to {device.deviceName}.");
        }
    }
}