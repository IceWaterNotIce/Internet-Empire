using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDevice", menuName = "Device")]
public class DeviceType : ScriptableObject
{
    public DeviceList.DeviceType deviceType;

    public int maxConnections = 1;

    public Sprite sprite;
}