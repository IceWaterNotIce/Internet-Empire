namespace InternetEmpire
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class Connection : MonoBehaviour
    {

        public Device Device1;
        public Device Device2;
        private ConnectionMethod connectionData;

        public ConnectionMethod ConnectionData
        {
            get { return connectionData; }
            set { connectionData = value; }
        }
        [SerializeField] private Color HoverColor = Color.red;
        [SerializeField] private Color SelectedColor = Color.green;
        private Color originalColor;
        private bool isClicked = false;

        [SerializeField] private PolygonCollider2D m_collider;

        [SerializeField] private SpriteRenderer spriteRenderer;

        void Start()
        {
            if (Device1 == null || Device2 == null)
            {
                Debug.LogWarning("Device1 or Device2 is null.");
                return;
            }
            // getting 2 devices' positions and controll gameobject's position, rotation and scale
            Vector2 device1Position = Device1.transform.position;
            Vector2 device2Position = Device2.transform.position;
            Vector2 direction = device2Position - device1Position;
            Vector2 center = (device1Position + device2Position) / 2;
            // z = 1 to make sure the line is in back of the devices
            transform.position = new Vector3(center.x, center.y, 1);
            transform.right = direction;
            transform.localScale = new Vector3(direction.magnitude/3, transform.localScale.y, transform.localScale.z);

            // set the connection's color to the original color
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;

            // enable the collider in start to resize the collider to fit the sprite
            m_collider.enabled = true;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = connectionData.sprite;
        }

        void Update()
        {
            if (isClicked && Keyboard.current.deleteKey.wasPressedThisFrame)
            {
                float moneyNeeded = connectionData.pricePerMeter * Vector2.Distance(Device1.transform.position, Device2.transform.position) / 5;
                CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
                if (cityStreetSceneManager.money >= moneyNeeded)
                {
                    cityStreetSceneManager.money -= moneyNeeded;
                }
                else
                {
                    MessageManager.Instance.ToastMessage("Not enough money to remove connection.");
                    return;
                }

                ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
                connectionManager.RemoveConnection(this);
                Destroy(gameObject);
            }
        }

        void OnMouseOver()
        {
            // change the connection's color when mouse is over the connection
            if (!isClicked)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.color = HoverColor;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                // select the connection when clicked
                isClicked = true;
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.color = SelectedColor;
            }
        }

        void OnMouseExit()
        {
            // change the connection's color back to the original color when mouse is not over the connection
            if (!isClicked)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.color = originalColor;
            }
        }
    }
}
