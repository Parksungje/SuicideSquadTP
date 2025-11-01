using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private SoundDataSO[] sounds;
    private readonly Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();

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
            if (sources.ContainsKey(key)) continue;
            var src = gameObject.AddComponent<AudioSource>();
            src.clip = s.clip;
            src.volume = s.volume;
            src.pitch = s.pitch;
            src.loop = s.loop;
            sources[key] = src;
        }
    }

    public void Play(string name)
    {
        if (!sources.TryGetValue(name, out var src))
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        src.Play();
    }

    public void Stop(string name)
    {
        if (!sources.TryGetValue(name, out var src))
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        src.Stop();
    }

    public void SetVolume(string name, float volume)
    {
        if (!sources.TryGetValue(name, out var src)) return;
        src.volume = Mathf.Clamp01(volume);
    }
}
