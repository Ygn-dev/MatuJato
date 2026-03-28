using System;
using UnityEngine;

public enum SoundType
{
    FOOTSTEP,
    ARRUGADO,
    REVIVE,
    ABRIR_DIALOGO,
    TIMBRE,
    ELEGIR_LLAVE,
    ABRIR_PUERTA,
    COGER_LLAVE,
    PUERTA_BLOQQUEADA,
    CLICK_JUGAR,
    GETTING_HIT,
    NEXT_DIALOGUE,
    ENTE_MOVIENDO
}

[Serializable]
public struct SoundList
{
    [HideInInspector] public string name;
    public AudioClip[] sounds;
}


[ExecuteInEditMode]
public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    public AudioSource audioSourcePrefab;
    [SerializeField] public SoundList[] soundList;
    
    //singleton pattern
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PlaySound(SoundType soundType, float volume = 1f)
    {
        //select random clip from the array
        AudioClip[] clips = soundList[(int)soundType].sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //spawn in  gameObject
        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        //assign the audioClip
        audioSource.clip = randomClip;
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();
        //get length of the clip
        float clipLength = randomClip.length;
        //destroy the audioSource after the clip has finished playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomPitch(SoundType soundType, float volume = 1f, float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        //select random clip from the array
        AudioClip[] clips = soundList[(int)soundType].sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //spawn in  gameObject
        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        //assign the audioClip
        audioSource.clip = randomClip;
        //assign random pitch
        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();
        //get length of the clip
        float clipLength = randomClip.length / audioSource.pitch;
        //destroy the audioSource after the clip has finished playing
        Destroy(audioSource.gameObject, clipLength);
    }

    public AudioSource GetRandomClip(SoundType soundType)
    {
        //select random clip from the array
        AudioClip[] clips = soundList[(int)soundType].sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //spawn in  gameObject
        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        //assign the audioClip
        audioSource.clip = randomClip;
        //return the audioSource
        return audioSource;
    }

    public AudioSource GetRandomClipWithPitch(SoundType soundType, float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        //select random clip from the array
        AudioClip[] clips = soundList[(int)soundType].sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        //spawn in  gameObject
        AudioSource audioSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity);
        //assign the audioClip
        audioSource.clip = randomClip;
        //assign random pitch
        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        //return the audioSource
        return audioSource;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++) soundList[i].name = names[i];
    }
#endif
}

