using UnityEngine;

public class Client : MonoBehaviour
{
    public string clientName;
    public ClientList.ClientType clientType;
    public Device device;

    public void Initialize(string name, ClientList.ClientType type, Device device)
    {
        clientName = name;
        clientType = type;
        this.device = device;
    }
}

