using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void RetryGame()
    {
        // Bellekteki gereksiz cache'leri temizle
        ClearCaches();

        // PiuZone2 sahnesine geç
        SceneManager.LoadScene("PiuZone2");
    }

    private void ClearCaches()
    {
        // Bütün static veri veya singleton referanslarını temizleyin
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        // DontDestroyOnLoad ile taşınan nesneler varsa temizleyin
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name == null)
            {
                Destroy(obj);
            }
        }
    }
}