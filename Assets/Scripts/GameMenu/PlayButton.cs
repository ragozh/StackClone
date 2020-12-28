using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void LoadGame(string GameScene)
    {
        if (GameScene == "") return;
        SceneManager.LoadScene(GameScene);
    }
}
