using UnityEngine;
using System.Collections.Generic;
using Unity.Services.CloudSave.Models.Data.Player;

namespace InternetEmpire
{

    public class SettingPanel : MonoBehaviour
    {

    }

    [System.Serializable]
    public class GameData
    {
        public Vector3 position;
        public Quaternion rotation;
        public object obj;
    }
    [System.Serializable]
public class GameDataList
{
    public List<GameData> datas;

    public GameDataList(List<GameData> datadtas)
    {
        this.datas = datadtas;
    }
}
}
