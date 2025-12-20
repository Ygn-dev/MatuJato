using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class GameInitiator : MonoBehaviour
{
    public ScriptableObject gameData;
    public GameObject character;
    public InputActionAsset inputActionAsset;
    public CinemachineCamera cameraBrain;
    public Canvas gameplayCanvas;
    public GameObject musica;
    public bool esPrimeraCarga = false;
    public float fadeOut = 3.5f;

    private Respawn respawnScript;
    private NivelDataBase nivelData;
    private GameObject confinerInst = null;
    private Vector3 dampbuff;
    private bool hayMusica = false;

    //singleton pattern
    public static GameInitiator instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public IEnumerator Start()
    {
        yield return StartCoroutine(LevantarPantallaCarga());
        yield return StartCoroutine(BindearDatos());
        yield return StartCoroutine(ColocarCamara());
        yield return StartCoroutine(ColocarPersonaje());
        yield return StartCoroutine(RenaudarElTiempo());
        yield return StartCoroutine(ActivarDamping());
        if(!esPrimeraCarga) yield return StartCoroutine(ActivarActionMaps());
        if(!esPrimeraCarga) yield return StartCoroutine(SoltarPantallaCarga(fadeOut));
        if(esPrimeraCarga) yield return StartCoroutine(primeraCarga());   
    }

    private IEnumerator primeraCarga()
    {
        yield return StartCoroutine(frameLevantarse());
        yield return StartCoroutine(SoltarPantallaCarga());
        yield return StartCoroutine(animacionLevantarse());
        
    }

    public IEnumerator continuacionCarga()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(ActivarActionMaps());
        yield return StartCoroutine(dialogoInicial());
    }

    private IEnumerator dialogoInicial()
    {
        GameObject triggerDialogoInicial = nivelData.triggerDialogoInicial;
        triggerDialogoInicial.GetComponent<DialogueTrigger>().TriggerDialogue();
        yield return null;
    }

    private IEnumerator frameLevantarse()
    {
        character.GetComponent<primeraCarga>().FrameLevantarse();
        yield return null;
    }

    private IEnumerator animacionLevantarse()
    {
        character.GetComponent<primeraCarga>().AnimacionLevantarse();
        yield return null;
    }

    public IEnumerator LevantarPantallaCarga()
    {
        gameplayCanvas.GetComponent<PantallaCarga>().PantallaNegro();
        yield return null;
    }

    private IEnumerator BindearDatos()
    {
        // obtener datos del nivel
        nivelData = (NivelDataBase)gameData;
        if (nivelData.nivelName == "Lobby_Data")
        {
            esPrimeraCarga = true;
        }
        else
        {
            if (!hayMusica)
            {
                Instantiate(musica);
                hayMusica = true;
            }
            esPrimeraCarga = false;
        }

        // obtener referencia al script respawn y bindear datos
        respawnScript = character.GetComponentInChildren<Respawn>();
        // cambiar el punto de respawn desde el scriptable object
        respawnScript.respawnPoint = nivelData.respawnPoint;

        yield return null;
    }

    private IEnumerator ColocarCamara()
    {
        // Obtener componentes necesarios
        dampbuff = cameraBrain.GetComponent<CinemachinePositionComposer>().Damping;

        cameraBrain.GetComponent<CinemachinePositionComposer>().Damping = Vector3.zero;
        cameraBrain.GetComponent<CinemachineConfiner2D>().SlowingDistance = 0f;
        cameraBrain.GetComponent<CinemachineConfiner2D>().Damping = 0f;

        //cambiar el zoom
        cameraBrain.Lens.OrthographicSize = nivelData.camaraZoom;
        // cambiar screen position composer
        cameraBrain.GetComponent<CinemachinePositionComposer>().Composition.ScreenPosition = nivelData.screenPositionComposer;
        if (nivelData.esDeadZone)
        {
            cameraBrain.GetComponent<CinemachinePositionComposer>().Composition.DeadZone.Enabled = true;
            cameraBrain.GetComponent<CinemachinePositionComposer>().Composition.DeadZone.Size = nivelData.deadZoneWidthHeight;
        }
        else
        {
            cameraBrain.GetComponent<CinemachinePositionComposer>().Composition.DeadZone.Enabled = false;
        }

        // Cambiar confiner
        if (confinerInst != null) Destroy(confinerInst);
        confinerInst = Instantiate(nivelData.confiner);
        cameraBrain.GetComponent<CinemachineConfiner2D>().BoundingShape2D = confinerInst.GetComponentInChildren<Collider2D>();
        cameraBrain.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache();
        yield return null;
    }

    private IEnumerator ColocarPersonaje()
    {
        // Colocar personaje
        character.transform.localScale = nivelData.escalaPersonaje;
        respawnScript.RespawnCharacter();
        character.GetComponent<PlayerController>().FirstAnim();
        cameraBrain.ForceCameraPosition(character.transform.position, cameraBrain.transform.rotation);
        cameraBrain.Follow = character.transform;

        yield return null;
    }

    private IEnumerator ActivarDamping()
    {
        cameraBrain.GetComponent<CinemachinePositionComposer>().Damping = dampbuff;
        cameraBrain.GetComponent<CinemachineConfiner2D>().SlowingDistance = nivelData.slowingDistance;
        cameraBrain.GetComponent<CinemachineConfiner2D>().Damping = nivelData.damping;
        yield return new WaitForFixedUpdate();
        yield return null;
    }

    public IEnumerator ActivarActionMaps()
    {
        var gameplayMap = inputActionAsset.FindActionMap("Gameplay");
        
        foreach (var action in gameplayMap.actions)
        {
            //if action es dialogue no activar
            if (action.name != "Dialogue/Accept") action.Enable();  
        }

        yield return null;
    }
    
    private IEnumerator RenaudarElTiempo()
    {
        Time.timeScale = 1f;
        yield return null;
    }

    public IEnumerator SoltarPantallaCarga(float fadeOut = 3.5f)
    {
        yield return gameplayCanvas.GetComponent<PantallaCarga>().FadeOut(fadeOut);
        if (esPrimeraCarga) yield return new WaitForSeconds(1f);
        yield return null;
    }
}
