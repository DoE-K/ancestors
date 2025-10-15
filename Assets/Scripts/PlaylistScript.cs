using UnityEngine;
using System.Collections;

public class PlaylistScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] tracks;

    private int currentTrackIndex = 0;

    void Start()
    {
        if (tracks.Length > 0)
        {
            PlayTrack(currentTrackIndex);
            StartCoroutine(PlayNextWhenFinished());
        }
    }

    void PlayTrack(int index)
    {
        audioSource.clip = tracks[index];
        audioSource.Play();
    }

    IEnumerator PlayNextWhenFinished()
    {
        while (true)
        {
            // Warte, bis der aktuelle Track zu Ende ist
            yield return new WaitWhile(() => audioSource.isPlaying);

            // Nächsten Track auswählen (zyklisch)
            currentTrackIndex = (currentTrackIndex + 1) % tracks.Length;
            PlayTrack(currentTrackIndex);
        }
    }
}
