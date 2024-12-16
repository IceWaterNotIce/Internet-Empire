using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class LobbySceneManager : MonoBehaviour
{
    [SerializeField] private GameObject LoginPanel;
    [SerializeField] private GameObject UserProfilePanel;
    [SerializeField] private GameObject SavedGamePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToCityStreet()
    {
        SceneManager.LoadScene("CityStreet");
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs Cleared");
        CloseSavedGamePanel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        GoToCityStreet();
    }
    public void ContinueGame()
    {
        GoToCityStreet();
    }

    public void OpenSavedGamePanel()
    {
        SavedGamePanel.SetActive(true);
    }

    public void CloseSavedGamePanel()
    {
        SavedGamePanel.SetActive(false);
    }

    public void CloseLoginPanel()
    {
        LoginPanel.SetActive(false);
    }

    public void CloseUserProfilePanel()
    {
        UserProfilePanel.SetActive(false);
    }

    


 

    public void user(){
        if (AuthenticationService.Instance.IsSignedIn)
        {
            if(UserProfilePanel.activeSelf)
            {
                UserProfilePanel.SetActive(false);
            }
            else
            {
                UserProfilePanel.SetActive(true);
                LoginPanel.SetActive(false);
            }
        }
        else
        {
            if(LoginPanel.activeSelf)
            {
                LoginPanel.SetActive(false);
            }
            else
            {
                LoginPanel.SetActive(true);
                UserProfilePanel.SetActive(false);
            }
        }
    }
}
