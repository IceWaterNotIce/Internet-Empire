using UnityEngine;
using System.Collections.Generic;

public class Device: MonoBehaviour
{
    public string deviceName;
    public DeviceManager.DeviceType deviceType;

    public List<Device> connectedDevices = new List<Device>();

    public int maxConnections = 1;

    public Material hoverMaterial;
    private Material originalMaterial;

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

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
    }

    void OnMouseOver()
    {
        // 懸停時更改材料
        if (hoverMaterial != null)
        {
            GetComponent<Renderer>().material = hoverMaterial;
        }
    }

    void OnMouseExit()
    {
        // 當鼠標不再懸停時恢復原始材料
        GetComponent<Renderer>().material = originalMaterial;
    }

}
