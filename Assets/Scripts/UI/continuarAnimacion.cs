using UnityEngine;
using System.Collections;



public class continuarAnimacion : MonoBehaviour
{
    public Animator animator;
    public Animator llave;
    public Animator logo;
    public Animator Texto;
    public GameObject fondoTapa;
    public AudioSource musicaFondo;
    
    public void ContinuarAnimacion()
    {
        animator.SetTrigger("Aparecer");
    }

    public void ContinuarAnimacion2()
    {
        llave.SetTrigger("Aparecer");
    }

    public void ContinuarAnimacion3()
    {
        fondoTapa.SetActive(true);
        animator.SetTrigger("LLave");
        logo.SetTrigger("Logo");
        Texto.SetTrigger("Aparecer");

        SoundFXManager.instance.PlaySound(SoundType.ABRIR_PUERTA);
        StartCoroutine(MusicaFondo());
    }

    private IEnumerator MusicaFondo()
    {
        yield return new WaitForSeconds(0.35f);
        musicaFondo.Play();
    }
}
