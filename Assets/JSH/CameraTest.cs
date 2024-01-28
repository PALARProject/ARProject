using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour
{

    public void SwitchScene()
    {
        // 현재 씬의 이름을 가져옵니다.
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 다음 씬의 이름을 설정합니다.
        string nextSceneName = (currentSceneName == "MainCameraScene") ? "ARCameraScene" : "MainCameraScene";

        // 씬 전환
        SceneManager.LoadScene(nextSceneName);
    }
}
