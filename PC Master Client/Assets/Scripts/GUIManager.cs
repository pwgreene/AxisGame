using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public CanvasGroup canvasGroup;
    public Text waveText;
	public Text waveStatusText;

    void Start()
    {
       
    }

    public void UpdateWaveNumber(int waveNumber)
    {
        waveText.text = "You survived " + (waveNumber - 1) + " waves!";
    }

	public void WaveStatusUpdate() {
		StartCoroutine("BlinkText", 0.3f);
	}

	 IEnumerator BlinkText(float delay) {
		for (int i = 0; i < 6; i++) {
			waveStatusText.text = "Next wave approaching";
			yield return new WaitForSeconds (delay);
			waveStatusText.text = "";
			yield return new WaitForSeconds (delay);
		}
	}

    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

	public void EndScreen(){
		StartCoroutine(ShowEndScreen());
	}

    public IEnumerator ShowEndScreen()
    {
		/*
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
		*/
		yield return new WaitForSeconds(1f);
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
		PhotonNetwork.LeaveLobby ();
        SceneManager.LoadScene(0);
    }
}
