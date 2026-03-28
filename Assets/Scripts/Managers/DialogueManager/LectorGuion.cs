using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;



public class LectorGuion : MonoBehaviour
{
    public GameObject prefab_TextBox;
    public GameObject prefab_TextBox2;
    public GameObject prefab_TextBox3;
    public float tiempoTransicionDialogos;
    public AnimationCurve curvaTransicionDialogos;
    public AnimationCurve curvaAparicionTexto;
    public float velocidadEscritura;
    public characterStats characterStats;

    private InputActionReference interact;
    private TMP_Text tmpText;
    private GameObject instTextBox;
    private List<GameObject> listaCajas;
    

    //singleton pattern
    public static LectorGuion instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public IEnumerator LeerGuion(TextAsset csvFile, GameObject dialoguesBox, InputActionReference inter)
    {
        interact = inter;
        string[] lines = csvFile.text.Split(new char[] { '\n' });
        for (int i = 1; i < lines.Length - 1; i++)
        {
            //por cada linea del guion
            string[] partes = lines[i].Split(';');
            int numero = int.Parse(partes[0]);
            string texto = partes[1];
            string personaje = partes[2];

            yield return moverYcrearNuevo(dialoguesBox, i,personaje);
            if(personaje.Trim() == "llave") SoundFXManager.instance.PlaySound(SoundType.COGER_LLAVE,0.8f);
            yield return LeerLinea(texto);
        }

        yield return null;
    }


    private IEnumerator moverYcrearNuevo(GameObject dialoguesBox, int index, string personaje)
    {
        //si es el primero, inicializar lista y salir
        if (index == 1)
        {
            //crear nueva caja de dialogo
            if(personaje.Trim() == "character") instTextBox = Instantiate(prefab_TextBox, dialoguesBox.transform);
            if(personaje.Trim() == "recepcionista") instTextBox = Instantiate(prefab_TextBox2, dialoguesBox.transform);

            //posicionar caja
            instTextBox.transform.localPosition = new Vector3(0, -90, 0);
            //obtener componentes
            tmpText = instTextBox.GetComponentInChildren<TMP_Text>();
            Image pedazoPapel = instTextBox.transform.GetChild(0).GetComponent<Image>();
            Image characterImage = instTextBox.transform.GetChild(1).GetComponent<Image>();


            //inicializar lista de cajas
            listaCajas = new List<GameObject> ();
            //animacion primero
            Animator animPapel = pedazoPapel.GetComponent<Animator>();
            
            // reproducir animación desde el inicio
            Color finalColorPapel = pedazoPapel.color;
            finalColorPapel.a = 1;
            pedazoPapel.color = finalColorPapel;

            animPapel.SetTrigger("PlayIntro");
            SoundFXManager.instance.PlaySound(SoundType.ABRIR_DIALOGO,0.4f);

            
            //finalizar animacion
            Color finalColorCharacter = characterImage.color;
            finalColorCharacter.a = 1;
            characterImage.color = finalColorCharacter;
            yield break;
        }
        else
        {
            //añadir cajas a la lista
            listaCajas.Add(instTextBox);


            //crear nueva caja de dialogo
            if(personaje.Trim() == "character") instTextBox = Instantiate(prefab_TextBox, dialoguesBox.transform);
            if(personaje.Trim() == "recepcionista") instTextBox = Instantiate(prefab_TextBox2, dialoguesBox.transform);
            if(personaje.Trim() == "llave")
            {
                characterStats.tieneLlave = true;
                instTextBox = Instantiate(prefab_TextBox3, dialoguesBox.transform);
            }

            //posicionar caja
            instTextBox.transform.localPosition = new Vector3(0, -90, 0);
            //obtener componentes
            tmpText = instTextBox.GetComponentInChildren<TMP_Text>();
            Image pedazoPapel = instTextBox.transform.GetChild(0).GetComponent<Image>();
            Image characterImage = instTextBox.transform.GetChild(1).GetComponent<Image>();


            //esperar accion del jugador para continuar
            yield return new WaitUntil(() => !interact.action.triggered);

            bool interrumpido = false;
            
            //posiciones iniciales y finales
            Vector3[] posicionesIniciales = new Vector3[listaCajas.Count];
            Vector3[] posicionesFinales = new Vector3[listaCajas.Count];
            //asignar posiciones
            for (int i = 0; i < listaCajas.Count; i++)
            {
                posicionesIniciales[i] = listaCajas[i].transform.localPosition;
                posicionesFinales[i] = new Vector3(listaCajas[i].transform.localPosition.x, listaCajas[i].transform.localPosition.y + 270, listaCajas[i].transform.localPosition.z);
            }

            SoundFXManager.instance.PlaySound(SoundType.NEXT_DIALOGUE,0.4f);

            float tiempo = 0f;

            while (tiempo < tiempoTransicionDialogos)
            {
                tiempo += Time.deltaTime;
                float t = Mathf.Clamp01(tiempo / tiempoTransicionDialogos);
                float curvaTD = curvaTransicionDialogos.Evaluate(t);
                float curvaA = curvaAparicionTexto.Evaluate(t);

                for (int i = 0; i < listaCajas.Count; i++)
                {
                    listaCajas[i].transform.localPosition = Vector3.LerpUnclamped(posicionesIniciales[i], posicionesFinales[i], curvaTD);
                    if (interact.action.triggered)
                    {
                        interrumpido = true;
                        break;
                    }
                }

                Color colorPapel = pedazoPapel.color;
                Color colorCharacter = characterImage.color;

                colorPapel.a = Mathf.LerpUnclamped(0, 1, curvaA);
                colorCharacter.a = Mathf.LerpUnclamped(0, 1, curvaA);

                pedazoPapel.color = colorPapel;
                characterImage.color = colorCharacter;

                if (interrumpido) break;
                yield return null;
            }

            for (int i = 0; i < listaCajas.Count; i++)
            {
                listaCajas[i].transform.localPosition = posicionesFinales[i];
            }
            Color finalColorPapel = pedazoPapel.color;
            Color finalColorCharacter = characterImage.color;
            finalColorPapel.a = 1;
            finalColorCharacter.a = 1;
            pedazoPapel.color = finalColorPapel;
            characterImage.color = finalColorCharacter;

            yield return new WaitUntil(() => !interact.action.triggered);
        }
        
    }

    private IEnumerator LeerLinea(string texto)
    {
        yield return MostrarTextoLinea(texto);
        yield return new WaitUntil(() => !interact.action.triggered);
        //esperar accion del jugador para continuar
        while (!interact.action.triggered)
        yield return null;
    }

    private IEnumerator MostrarTextoLinea(string texto)
    {
        int totalCaracteres = texto.Length;
        int caracteresMostrados = 0;
        float delay = 1f / velocidadEscritura;

        while (caracteresMostrados < totalCaracteres)
        {
            if (interact.action.triggered)
            {
                tmpText.text = texto; // Asegura que todo el texto esté visible al final
                yield break;
            }
            // Incrementa según velocidad (caracteres por segundo)

            tmpText.text = texto.Substring(0, caracteresMostrados);
            caracteresMostrados++;

            float timer = 0f;
            while (timer < delay)
            {
                if (interact.action.triggered)
                {
                    tmpText.text = texto;
                    yield break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        tmpText.text = texto; // Asegura que todo el texto esté visible al final
    }
}
