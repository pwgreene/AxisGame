using UnityEngine;
using System.Collections;

public class RotatingCoreBehaviour : MonoBehaviour
{
    public float startingHP;
    public float currentHP;
    public float rotationSpeed;

    public float minAlarmTime;
    public float maxAlarmTime;
    public float minVolume;
    public float maxVolume;

    const float LINE_WIDTH = .2F;
    public GameObject coreExplosion;
    public Spoke rod;

    SpriteRenderer coreSprite;
    PhotonView pv;
    AudioSource audioSource;

    void Start()
    {
        currentHP = startingHP;
        coreSprite = GetComponent<SpriteRenderer>();
        pv = PhotonView.Get(this);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DangerBeep());
    }

    IEnumerator DangerBeep()
    {
        while (true)
        {
            if (currentHP < startingHP)
            {
                while (true)
                {
                    audioSource.volume = Mathf.Lerp(minVolume, maxVolume, (startingHP - currentHP) / startingHP);
                    audioSource.Play();

                    float timeInterval = Mathf.Lerp(maxAlarmTime, minAlarmTime, (startingHP - currentHP) / startingHP);
                    yield return new WaitForSeconds(timeInterval);
                }
            }

            yield return null;
        }
    }

    void FixedUpdate()
    {
        this.gameObject.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    [PunRPC]
    public void CoreDamage(int damageValue)
    {
        currentHP -= (float)damageValue;
        if (null == coreSprite)
        {
            coreSprite = GetComponent<SpriteRenderer>();
        }
        coreSprite.color = new Color(1, 1 - (float)(startingHP - currentHP) / startingHP, 1 - (float)(startingHP - currentHP) / startingHP);

        if (currentHP < 0)
        {
            EndGame();

            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


    public void EndGame()
    {
        Instantiate(coreExplosion, transform.position, transform.rotation);
        GameObject gui = GameObject.FindGameObjectWithTag("GUIManager");
        gui.GetComponent<GUIManager>().EndScreen();
    }

    public void Damage(int damageValue)
    {
        if (null == pv)
        {
            pv = PhotonView.Get(this);
        }

        pv.RPC("CoreDamage", PhotonTargets.AllBufferedViaServer, damageValue);
    }
}