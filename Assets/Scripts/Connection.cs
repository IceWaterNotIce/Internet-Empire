using UnityEngine;

public class Connection : MonoBehaviour
{
    public DeviceController Device1;
    public DeviceController Device2;

    public ConnectionManager.ConnectionMethod connectionType;

    void Start()
    {
        // getting 2 devices' positions and controll gameobject's position, rotation and scale
        Vector2 device1Position = Device1.transform.position;
        Vector2 device2Position = Device2.transform.position;
        Vector2 direction = device2Position - device1Position;
        Vector2 center = (device1Position + device2Position) / 2;
        transform.position = center;
        transform.right = direction;
        transform.localScale = new Vector3(direction.magnitude, 1, 1);
    }
}