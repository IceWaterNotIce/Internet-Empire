using UnityEngine;

[CreateAssetMenu(fileName = "NewClient", menuName = "Client")]
public class Client : ScriptableObject
{
    public string clientName;
    public ClientList.ClientType clientType;
    private Device device;

    public Device Device
    {
        get { return device; }
        set { device = value; }
    }

    public int satisfaction;
}

