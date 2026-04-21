using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AutoDeselectButton : MonoBehaviour, ISelectHandler
{
    public float delay = 1f;

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(DeselectAfterDelay());
    }

    private IEnumerator DeselectAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
