using UnityEngine;

[CreateAssetMenu(fileName = "DeviceList", menuName = "DeviceList")]
public class DeviceList : ScriptableObject
{
    public enum DeviceType { Computer, Router, Switch, Server }
    public Device[] devices; // 裝置列表

    public Device GetDevice(int index)
    {
        // Assume No = index + 1
        if (index < 0 || index >= this.devices.Length)
        {
            Debug.LogError("Index out of range.");
            return null;
        }
        return this.devices[index];
    }

    public int GetCount()
    {
        return this.devices.Length;
    }
}