using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene("__Scene_0");
    }
}
