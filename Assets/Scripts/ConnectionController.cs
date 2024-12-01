using UnityEngine;

public class ConnectionController : MonoBehaviour
{

    public DeviceController Device1;
    public DeviceController Device2;
    private Connection connectionData;

    public Connection ConnectionData
    {
        get { return connectionData; }
        set { connectionData = value; }
    }

    [SerializeField] private Color HoverColor = Color.red;
    private Color originalColor;

    [SerializeField] private PolygonCollider2D m_collider;

    void Start()
    {
        // getting 2 devices' positions and controll gameobject's position, rotation and scale
        Vector2 device1Position = Device1.transform.position;
        Vector2 device2Position = Device2.transform.position;
        Vector2 direction = device2Position - device1Position;
        Vector2 center = (device1Position + device2Position) / 2;
        // z = 1 to make sure the line is in back of the devices
        transform.position = new Vector3(center.x, center.y, 1);
        transform.right = direction;
        transform.localScale = new Vector3(direction.magnitude, 1, 1);

        // set the connection's color to the original color
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // enable the collider in start to resize the collider to fit the sprite
        m_collider.enabled = true;
    }

    void OnMouseOver()
    {
        // change the connection's color when mouse is over the connection
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = HoverColor;
    }

    void OnMouseExit()
    {
        // change the connection's color back when mouse is not over the connection
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = originalColor;
    }
}
