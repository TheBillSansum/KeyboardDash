using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song Data")]
public class SongData : ScriptableObject
{
    public string title;
    public string artist;
    public Sprite albumCover;
    public AudioClip audioClip;
}
