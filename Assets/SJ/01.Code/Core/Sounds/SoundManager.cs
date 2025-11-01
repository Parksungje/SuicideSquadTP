using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private SoundDataSO[] sounds;
    private readonly Dictionary<string, SoundDataSO> soundDataDict = new Dictionary<string, SoundDataSO>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (sounds == null) return;

        foreach (var s in sounds)
        {
            if (s == null || s.clip == null) continue;
            var key = s.name;
            if (!soundDataDict.ContainsKey(key))
                soundDataDict[key] = s;
        }
    }

    public void Play(string name)
    {
        if (!soundDataDict.TryGetValue(name, out var data))
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        var tempObj = new GameObject("Audio_" + name);
        var src = tempObj.AddComponent<AudioSource>();
        src.clip = data.clip;
        src.volume = data.volume;
        src.pitch = data.pitch;
        src.loop = data.loop;

        src.Play();
        if (!src.loop)
            Destroy(tempObj, data.clip.length / src.pitch);
    }

    public void Stop(string name)
    {
        var objs = GameObject.FindObjectsOfType<AudioSource>();
        foreach (var src in objs)
        {
            if (src == null || src.clip == null) continue;
            if (src.clip.name == name)
            {
                src.Stop();
                Destroy(src.gameObject);
            }
        }
    }

    public void SetVolume(string name, float volume)
    {
        var objs = GameObject.FindObjectsOfType<AudioSource>();
        foreach (var src in objs)
        {
            if (src == null || src.clip == null) continue;
            if (src.clip.name == name)
                src.volume = Mathf.Clamp01(volume);
        }
    }
}
