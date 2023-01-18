using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
    [SerializeField] private float sceneLoadDelay = 2f;
    private ScoreKeeper _scoreKeeper;

    private void Awake(){
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void LoadGame(){
        _scoreKeeper.ResetScore();
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameOver(){
        StartCoroutine(WaitAndLoad("GameOver", sceneLoadDelay));
    }

    public void QuitGame(){
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    private IEnumerator WaitAndLoad(string sceneName, float delay){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}