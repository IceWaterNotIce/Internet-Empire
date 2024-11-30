using UnityEngine;

[CreateAssetMenu(fileName = "NewConnection", menuName = "Connection")]
public class Connection : ScriptableObject
{


    public int pricePerMeter;
    public int maxSpeed;
    public int latency;
    public int reliability;
    public int security;
    public int maxDistance;

    public Sprite sprite;

}