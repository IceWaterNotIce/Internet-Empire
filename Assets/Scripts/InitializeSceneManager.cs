using UnityEngine;
using UnityEngine.SceneManagement;

namespace InternetEmpire
{
    public class InitializeSceneManager : MonoBehaviour
    {
        public bool isAssetBundleReady = false;
        public bool isVersionChecked = false;
        void Start()
        {
            // Check for updates
            // UpdateChecker updateChecker = new UpdateChecker();
            // updateChecker.CheckForUpdate();
            // Check for version
            // VersionChecker versionChecker = new VersionChecker();
            // versionChecker.CheckForUpdate();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Check()
        {
            if (isAssetBundleReady && isVersionChecked)
            {
                // Load the lobby scene
                SceneManager.LoadScene("Lobby");
            }
        }

        public void OnClickQuit()
        {
            Application.Quit();
        }
    }
}