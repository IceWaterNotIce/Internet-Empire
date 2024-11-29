using UnityEngine;

public class Connection : MonoBehaviour
{
    public Device Device1;
    public Device Device2;

    public ConnectionManager.ConnectionMethod connectionType;

    void Start()
    {
        // getting 2 devices' positions and controll gameobject's position, rotation and scale
        Vector3 device1Position = Device1.transform.position;
        Vector3 device2Position = Device2.transform.position;
        Vector3 connectionPosition = (device1Position + device2Position) / 2;
        transform.position = connectionPosition;

        Vector3 direction = device2Position - device1Position;
        transform.rotation = Quaternion.LookRotation(direction);
        // 添加 90 度到 Y 軸的旋轉
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y + 90, 0);

        float distance = Vector3.Distance(device1Position, device2Position);
        transform.localScale = new Vector3(distance, 0.2f, 1);
    }
}