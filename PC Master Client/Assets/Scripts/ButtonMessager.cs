using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMessager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public byte eventCode;
    public float contentOnDown;
    public float contentOnUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        PhotonNetwork.RaiseEvent(eventCode, contentOnDown, true, null);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PhotonNetwork.RaiseEvent(eventCode, contentOnUp, true, null);
    }
}
