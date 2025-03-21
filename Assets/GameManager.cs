using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{

    public float playerKill;
    public float playerMoveDistance;

    public bool playerDead;

    public void Start()
    {
        Application.targetFrameRate = 60;
    }


    public void LoadStartScene()
    {
        playerKill = 0;
        playerMoveDistance = 0;
        playerDead = false;
        SceneManager.LoadScene("StartScene");
        LoadUI(SceneManager.GetSceneByName("StartScene").buildIndex);

        DataManager.Instance.SaveData();
        AudioManager.Instance.PlayBGM(AudioManager.BGM.BGM_TITLE);
    }

    public void LoadGameScene()
    {

        //DataManager.Instance.LoadData();

        UIManager.Instance.init = true;
        SceneManager.LoadScene("GameScene");
        LoadUI(SceneManager.GetSceneByName("GameScene").buildIndex);
        AudioManager.Instance.PlayBGM(AudioManager.BGM.BGM_INGAME);
    }

    private void LoadUI(int index)
    {
        UIManager.Instance.LoadCurrentSceneUI(index);
    }

}
