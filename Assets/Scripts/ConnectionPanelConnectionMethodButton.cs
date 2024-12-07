using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace InternetEmpire
{
    public class ConnectionPanelConnectionMethodButton : MonoBehaviour
    {
        private Connection m_connection;
        public Connection Connection
        {
            get { return m_connection; }
            set { m_connection = value; }
        }

        [SerializeField] private TMP_Text m_tmpPrice;
        [SerializeField] private Image m_imgPreview;
        [SerializeField] private Button m_btnBuy;

        private void Start()
        {
            m_tmpPrice.text = m_connection.pricePerMeter.ToString();
            m_imgPreview.sprite = m_connection.sprite;
            m_btnBuy.onClick.AddListener(() =>
            {
                CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
                cityStreetSceneManager.money -= m_connection.pricePerMeter;
                ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
                connectionManager.currentMethod = m_connection;
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