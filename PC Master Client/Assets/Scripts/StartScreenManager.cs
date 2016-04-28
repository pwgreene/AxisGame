using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScreenManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
