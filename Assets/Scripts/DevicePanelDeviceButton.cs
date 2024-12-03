using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace InternetEmpire
{
    public class DevicePanelDeviceButton : MonoBehaviour
    {
        private DeviceModel m_deviceModel;
        public DeviceModel DeviceModel
        {
            get { return m_deviceModel; }
            set { m_deviceModel = value; }
        }

        [SerializeField] private TMP_Text m_tmpPrice;
        [SerializeField] private Image m_imgPreview;
        [SerializeField] private Button m_btnBuy;

        private void Start()
        {
            m_tmpPrice.text = m_deviceModel.Price.ToString();
            m_imgPreview.sprite = m_deviceModel.Sprite;
            m_btnBuy.onClick.AddListener(() =>
            {
                CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
                cityStreetSceneManager.money -= m_deviceModel.Price;
                DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();
                deviceManager.PlayerDevice = m_deviceModel;
            });
        }

        private void OnMouseOver()
        {
        }

        private void OnMouseExit()
        {

        }


    }
}