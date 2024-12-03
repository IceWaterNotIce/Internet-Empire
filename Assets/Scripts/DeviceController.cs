namespace InternetEmpire
{
    using UnityEngine;
    using UnityEngine.UI;

    public class DeviceController : MonoBehaviour
    {
        private global::DeviceType deviceData;

        public global::DeviceType DeviceData
        {
            get { return deviceData; }
            set { deviceData = value; }
        }

        private Color originalColor;

        [SerializeField] private GameObject ConnectionStateField;
        [SerializeField] private GameObject ConnectionState;

        public int connectionsCount = 0;
        public int ConnectionsCount
        {
            get { return connectionsCount; }
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Connections count cannot be less than 0.");
                    connectionsCount = 0;
                }
                else if (value > deviceData.maxConnections)
                {
                    Debug.LogError("Connections count cannot be greater than max connections.");
                    connectionsCount = deviceData.maxConnections;
                }
                connectionsCount = value;
                UpdateUI();
            }
        }

        void Start()
        {
            originalColor = GetComponent<Renderer>().material.color;
            GetComponent<SpriteRenderer>().sprite = deviceData.sprite;
            UpdateUI();
        }

        void OnMouseOver()
        {
            GetComponent<SpriteRenderer>().material.color = Color.yellow;
        }

        void OnMouseExit()
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }

        void UpdateUI()
        {
            for (int i = ConnectionStateField.transform.childCount; i < deviceData.maxConnections; i++)
            {
                Instantiate(ConnectionState, ConnectionStateField.transform);
            }
            for (int i = 0; i < deviceData.maxConnections; i++)
            {
                ConnectionStateField.transform.GetChild(i).GetComponent<RawImage>().color = i < connectionsCount ? Color.green : Color.white;
            }
        }
    }
}