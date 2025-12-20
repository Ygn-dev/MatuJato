using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Must;

public class TransicionNiveles : MonoBehaviour
{
    public ScriptableObject nivelDestino;
    public PantallaCarga PantallaCarga;
    public float duracion;
    public characterStats characterStats;


    public void CambiarNivel()
    {
        if(!characterStats.tieneLlave)
        {
            SoundFXManager.instance.PlaySound(SoundType.PUERTA_BLOQQUEADA,0.7f);
        }
        if(characterStats.tieneLlave)
        {
           StartCoroutine(Transicion());
        } 
    }

    private IEnumerator Transicion()
    {
        SoundFXManager.instance.PlaySound(SoundType.ABRIR_PUERTA,0.7f);
        yield return StartCoroutine(PantallaCarga.FadeIn(duracion));
        GameInitiator.instance.gameData = nivelDestino;
        GameInitiator.instance.fadeOut = duracion;
        yield return StartCoroutine(GameInitiator.instance.Start()); 
    }

    public void siempreCerrado()
    {
        SoundFXManager.instance.PlaySound(SoundType.PUERTA_BLOQQUEADA,0.7f);
    }
}

