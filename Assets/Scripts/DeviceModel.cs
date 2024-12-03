using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDevice", menuName = "Device")]
public class DeviceModel : ScriptableObject
{
    [SerializeField]
    private DeviceTypeList.DeviceType m_deviceType;
    public DeviceTypeList.DeviceType Type
    {
        get { return m_deviceType; }
        set { m_deviceType = value; }
    }

    [SerializeField]
    private int m_price = 100;
    public int Price
    {
        get { return m_price; }
        set { m_price = value; }
    }

    [SerializeField]
    private int m_maxConnections = 1;
    public int MaxConnections
    {
        get { return m_maxConnections; }
        set { m_maxConnections = value; }
    }

    [SerializeField]
    private int m_capacity = 100;
    public int Capacity
    {
        get { return m_capacity; }
        set { m_capacity = value; }
    }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }
}