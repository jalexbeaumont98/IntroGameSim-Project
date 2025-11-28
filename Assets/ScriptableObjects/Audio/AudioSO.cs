using UnityEngine;

[CreateAssetMenu(menuName = "Audio/SoundEffect")]
public class AudioSO : ScriptableObject
{
    public int id;
    public AudioClip clip;
}
