using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void OnClicked()
    {
        Debug.Log("WinScreen clicked");
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
