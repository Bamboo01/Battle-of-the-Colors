using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Bamboo.Utility;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioclip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 10f)]
    public float pitch = 1f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}


public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio files")]
    [SerializeField] List<Sound> soundClips = new List<Sound>();

    public Dictionary<int, Sound> IDToSoundClip = new Dictionary<int, Sound>();
    private Dictionary<string, int> NameToID = new Dictionary<string, int>();
    private AudioSource audioSource;

    #region core functions
    protected override void OnAwake()
    {
        //DontDestroyOnLoad(this.gameObject);
        int ID = 0;
        SceneManager.sceneUnloaded += ChangeScene;
        foreach (Sound clip in soundClips)
        {
            clip.source = gameObject.AddComponent<AudioSource>();
            clip.source.clip = clip.audioclip;
            clip.source.volume = clip.volume;
            clip.source.pitch = clip.pitch;
            clip.source.loop = clip.loop;

            NameToID.Add(clip.name, ID);
            IDToSoundClip.Add(ID, clip);
            ID++;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void ChangeScene(Scene arg0)
    {
        StopAllSounds();
    }

    public void ForcePlaySoundByID(int ID)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(ID, out clip))
        {
            clip.source.Play();
        }
        else
        {
            Debug.LogError("Failed to get sound with ID " + ID);
        }
    }

    public void PlaySoundByID(int ID)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(ID, out clip))
        {
            if (!clip.source.isPlaying)
            {
                clip.source.volume = clip.volume;
                clip.source.Play();
            }
        }
        else
        {
            Debug.LogError("Failed to get sound with ID " + ID);
        }
    }

    public void PlaySoundByName(string Name)
    {
        int ID;
        if (NameToID.TryGetValue(Name, out ID))
        {
            Sound clip = IDToSoundClip[ID];
            if (!clip.source.isPlaying)
            {
                clip.source.volume = clip.volume;
                clip.source.Play();
            }
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
        }
    }

    public void ForcePlaySoundByName(string Name)
    {
        int ID;
        if (NameToID.TryGetValue(Name, out ID))
        {
            Sound clip = IDToSoundClip[ID];
            clip.source.volume = clip.volume;
            clip.source.Play();
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
        }
    }

    public void StopSoundByID(int ID)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(ID, out clip))
        {
            clip.source.Stop();
        }
        else
        {
            Debug.LogError("Failed to get sound with ID " + ID);
        }
    }

    public void StopSoundByName(string Name)
    {
        int ID;
        if (NameToID.TryGetValue(Name, out ID))
        {
            IDToSoundClip[ID].source.Stop();
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound clip in soundClips)
        {
            clip.source.Stop();
        }
    }

    public void PauseSoundByID(int ID)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(ID, out clip))
        {
            clip.source.Pause();
        }
        else
        {
            Debug.LogError("Failed to get sound with ID " + ID);
        }
    }

    public void PauseSoundByName(string Name)
    {
        int ID;
        if (NameToID.TryGetValue(Name, out ID))
        {
            IDToSoundClip[ID].source.Pause();
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
        }
    }

    public void PauseAllSounds()
    {
        foreach (Sound clip in soundClips)
        {
            clip.source.Pause();
        }
    }

    public int GetSoundID(string name)
    {
        return NameToID[name];
    }

    public void UpdateSoundPitchByID(int ID, float pitch)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(ID, out clip))
        {
            clip.pitch = Mathf.Clamp(pitch, 0.1f, 10.0f);
            clip.source.pitch = Mathf.Clamp(pitch, 0.1f, 10.0f);
        }
        else
        {
            Debug.LogError("Failed to get sound with ID " + ID);
        }
    }

    public void UpdateSoundPitchByName(string Name, float pitch)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(GetSoundID(Name), out clip))
        {
            clip.pitch = Mathf.Clamp(pitch, 0.1f, 10.0f);
            clip.source.pitch = Mathf.Clamp(pitch, 0.1f, 10.0f);
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
        }
    }

    public void UpdateSoundVolumeByID(int ID, float Volume)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(ID, out clip))
        {
            clip.volume = Mathf.Clamp(Volume, 0f, 1.0f);
            clip.source.volume = Mathf.Clamp(Volume, 0f, 1.0f);
        }
        else
        {
            Debug.LogError("Failed to get sound with ID " + ID);
        }
    }

    public void UpdateSoundVolumeByName(string Name, float Volume)
    {
        Sound clip;
        if (IDToSoundClip.TryGetValue(GetSoundID(Name), out clip))
        {
            clip.volume = Mathf.Clamp(Volume, 0f, 1.0f);
            clip.source.volume = Mathf.Clamp(Volume, 0f, 1.0f);
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
        }
    }
    #endregion

    public AudioSource PlaySoundAtPointByName(string Name, Vector3 position, bool oneshot = true, float spatialblend = 1.0f)
    {
        int ID;
        if (NameToID.TryGetValue(Name, out ID))
        {
            // Get the clip
            Sound clip = IDToSoundClip[ID];
            GameObject pointAudio = ObjectPool.Instance.spawnFromPool("PointAudio");
            AudioSource audioSource = pointAudio.GetComponent<AudioSource>();
            audioSource.clip = clip.audioclip;
            audioSource.volume = clip.volume;
            audioSource.pitch = clip.pitch;
            audioSource.loop = !oneshot;
            audioSource.spatialBlend = spatialblend;
            audioSource.Play();

            if (oneshot)
            {
                audioSource.gameObject.SetActiveDelayed(false, audioSource.clip.length);
            }

            pointAudio.transform.position = position;
            return audioSource;
        }
        else
        {
            Debug.LogError("Failed to get sound with name " + Name);
            return null;
        }
    }
}