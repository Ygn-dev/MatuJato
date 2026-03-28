
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MainMenuEvents : MonoBehaviour
{
    public bool estamosEnAnimacionInicial = true;
    public InputActionReference accept;
    public Animator cajaAnimator;
    public Animator textoAnimator;

    public GameObject fade;
    void Start()
    {
        accept.action.performed += OnAcceptPerformed;
    }

    private void OnAcceptPerformed(InputAction.CallbackContext context)
    {
        if (estamosEnAnimacionInicial)
        {
            cajaAnimator.SetTrigger("Continuar");
            textoAnimator.SetTrigger("Continuar");
        }
    }

    public void NuevoJuego()
    {
        StartCoroutine(IniciarNuevoJuego());
    }

    private IEnumerator IniciarNuevoJuego()
    {
        fade.SetActive(true);
        SoundFXManager.instance.PlaySound(SoundType.CLICK_JUGAR);
        yield return StartCoroutine(fade.GetComponent<PantallaCarga>().FadeIn(1f));
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ZonaCamaras");
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}