using UnityEngine;
using UnityEngine.UI;

namespace InternetEmpire
{
    public class DevicePanelController : MonoBehaviour
    {
        [SerializeField] private DeviceTypeList DeviceList;
        [SerializeField] private GameObject DeviceButtonPrefab;

        [SerializeField] private DeviceManager DeviceManager;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            foreach (DeviceModel Device in DeviceList.devices)
            {
                Sprite DevicePreviewSprite = Device.Sprite;
                GameObject DeviceButton = Instantiate(DeviceButtonPrefab, transform);
                Image image = DeviceButton.GetComponent<Image>();
                image.sprite = DevicePreviewSprite;

                // Add a button click listener
                Button button = DeviceButton.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    DeviceManager.PlayerDevice = Device;
                });
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}