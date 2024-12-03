using UnityEngine;
using System.Collections.Generic;
namespace InternetEmpire
{

    public class DeviceManager : MonoBehaviour
    {
        public GameObject devicePrefab; // 裝置預製件
        public DeviceTypeList deviceList; // 參考 DeviceList

        public List<Device> devices = new List<Device>();

        public DeviceModel PlayerDevice;

        void Start()
        {

        }

        void Update()
        {
            if(PlayerDevice != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;
                    GenerateDevices(PlayerDevice, mousePosition);
                    PlayerDevice = null;
                }
            }
        }

        public GameObject GenerateDevices(DeviceModel model, Vector3 spawnPosition)
        {
            GameObject deviceObj = Instantiate(devicePrefab, spawnPosition, Quaternion.identity);
            Device deviceController = deviceObj.GetComponent<Device>();
            deviceController.Model = model;
            devices.Add(deviceController);
            return deviceObj;
        }

        private DeviceModel GetDeviceData(DeviceTypeList.DeviceType deviceType)
        {
            foreach (DeviceModel device in deviceList.devices)
            {
                if (device.Type == deviceType)
                {
                    return device;
                }
            }
            Debug.LogWarning($"Device with type {deviceType} not found in DeviceList.");
            return null;
        }

        public void RemoveDevice(Device device)
        {
            devices.Remove(device);
            Destroy(device.gameObject);
        }
    }
}