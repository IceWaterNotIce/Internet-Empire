using UnityEngine;

[CreateAssetMenu(fileName = "DeviceList", menuName = "DeviceList")]
public class DeviceTypeList : ScriptableObject
{
    public enum DeviceType { Computer, Router, Switch, Server }
    public global::DeviceModel[] devices; // 裝置列表

    public global::DeviceModel GetDevice(int index)
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