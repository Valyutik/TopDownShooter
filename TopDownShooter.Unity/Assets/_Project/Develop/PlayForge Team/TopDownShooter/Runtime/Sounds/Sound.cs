using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Sounds
{
    [System.Serializable]
    public class Sound
    {
        [SerializeField] private SoundType type;
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volume;

        public SoundType Type => type;
        public AudioClip Clip => clip;
        public float Volume => volume;
    }
}