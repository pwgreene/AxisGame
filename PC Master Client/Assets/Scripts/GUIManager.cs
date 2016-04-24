using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Text waveText;

    public void Start()
    {
        StartCoroutine(ShowEndScreen());
    }

    public void UpdateWaveNumber(int waveNumber)
    {
        waveText.text = "You survived " + (waveNumber - 1) + " waves!";
    }

    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    public IEnumerator ShowEndScreen()
    {
        GameObject core = null;
        while (core == null)
        {
            core = GameObject.FindGameObjectWithTag("Core");
            yield return null;
        }

        while (core != null)
        {
            yield return null;
        }

        bool go = true;
        while (go)
        {
            if (Time.timeScale > 0.05f)
            {
                // Slow down time for sicknasty effects.
                Time.timeScale -= 0.05f;
                canvasGroup.alpha += 0.05f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
            else
            {
                Time.timeScale = 0.001f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                canvasGroup.alpha = 1;
                go = false;
            }

            yield return new WaitForSeconds(0.05f * Time.timeScale);
        }

        yield return new WaitForSeconds(5 * Time.timeScale);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
