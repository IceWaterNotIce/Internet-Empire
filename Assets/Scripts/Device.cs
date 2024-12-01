using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDevice", menuName = "Device")]
public class Device : ScriptableObject
{
    public string deviceName;
    public DeviceList.DeviceType deviceType;

    public int maxConnections = 1;

    public Sprite sprite;
}