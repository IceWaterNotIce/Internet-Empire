using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbySceneManager : MonoBehaviour
{
    private BundleLoader bundleLoader;


    // Start is called before the first frame update
    void Start()
    {
        bundleLoader = GameObject.Find("BundleLoader").GetComponent<BundleLoader>();
    }

    public void GoToCityStreet()
    {
        SceneManager.LoadScene("CityStreet");
    }
}


