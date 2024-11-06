using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class MainMenu : MonoBehaviour
{
    public static bool IsHardMode { get; private set; } = false;
    public static bool IsMediumMode { get; private set; } = false;
    public void LoadEasyLevel()
    {
        //CODE UPDATE
        // SceneLoader.Instance.LoadScene(SceneType.Easy);
        SceneManager.LoadScene("Easy");
    }

    public void LoadMediumLevel()
    {
        //CODE UPDATE
        IsMediumMode = true;
        // SceneLoader.Instance.LoadScene(SceneType.Medium);
        SceneManager.LoadScene("Medium");
        UnityEngine.Debug.Log("Menu Selected Medium Mode: " + IsMediumMode);
    }

    public void LoadHardLevel()
    {
        //CODE UPDATE
        IsHardMode = true;
        // SceneLoader.Instance.LoadScene(SceneType.Hard);
        SceneManager.LoadScene("Hard");

        UnityEngine.Debug.Log("Menu Selected Hard Mode: " + IsHardMode);
    }

}
