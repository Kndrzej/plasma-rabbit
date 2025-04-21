using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreField;
    public void OnClicked()
    {
        Debug.Log("WinScreen clicked");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void SetScore(int score)
    {
        _scoreField.text = "You got "+score.ToString()+" points!";
    }
}
