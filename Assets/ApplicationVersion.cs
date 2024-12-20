using TMPro;
using UnityEngine;

public class ApplicationVersion : MonoBehaviour
{
    [SerializeField] private TMP_Text TmpApplicationVersion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TmpApplicationVersion.text = "Version: " + Application.version;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
