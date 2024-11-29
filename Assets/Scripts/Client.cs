using UnityEngine;

public class Client : MonoBehaviour
{
    public string clientName;
    public ClientManager.ClientType clientType;
    public Device device;

    public void Initialize(string name, ClientManager.ClientType type, Device device)
    {
        clientName = name;
        clientType = type;
        this.device = device;
    }
}
