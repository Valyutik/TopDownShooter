using UnityEngine;

namespace PlayForge_Team.TopDownShooter.Runtime.Sounds
{
    public sealed class WeaponSound : MonoBehaviour
    {
        [SerializeField] private Sound[] sounds;
        private AudioSource _audioSource;

        public void Init()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlaySound(SoundType type)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            foreach (var sound in sounds)
            {
                if (sound.Type == type)
                {
                    _audioSource.PlayOneShot(sound.Clip, sound.Volume);
                }
            }
        }
    }
}