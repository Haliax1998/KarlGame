using UnityEngine;

public static class AudioLoader
{
    private static AudioSource GetSource(string name)
    {
        var obj = GameObject.Find(name);
        if (obj == null)
        {
            Debug.LogError($"GameObject '{name}' no encontrado");
            return null;
        }
        return obj.GetComponent<AudioSource>();
    }

    public static void PlayBackground(string audioName)
    {
        var source = GetSource("BackgroundAudio");
        if (source == null) return;

        var clip = Resources.Load<AudioClip>($"Story/{StoryManager.SelectedStory}/Audio/{audioName}");
        if (clip != null)
        {
            source.Stop();
            source.loop = true;
            source.clip = clip;
            source.Play();
        }
    }

    public static void StopBackground()
    {
        var source = GetSource("BackgroundAudio");
        source?.Stop();
    }

    public static void PlayVoice(string audioName)
    {
        var source = GetSource("VoiceAudio");
        if (source == null) return;

        var clip = Resources.Load<AudioClip>($"Story/{StoryManager.SelectedStory}/Audio/{audioName}");
        if (clip != null)
        {
            source.Stop();
            source.loop = false;
            source.clip = clip;
            source.Play();
        }
    }

    public static void StopVoice()
    {
        var source = GetSource("VoiceAudio");
        source?.Stop();
    }
}
