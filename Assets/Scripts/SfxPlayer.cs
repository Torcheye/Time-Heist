using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SfxPlayer : MonoBehaviour
{
    public List<AudioClip> footsteps;
    public List<AudioClip> Rfootsteps;
    public AudioClip collectChip;
    public AudioClip shoot;
    public AudioClip bulletHit;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip shootR;
    public AudioClip bulletHitR;
    public AudioClip insert;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep(bool reverse = false)
    {
        _audioSource.PlayOneShot(reverse ? 
            Rfootsteps[Random.Range(0, footsteps.Count)] : footsteps[Random.Range(0, Rfootsteps.Count)]);
    }

    public void PlayCollectChip()
    {
        _audioSource.PlayOneShot(collectChip);
    }
    
    public void PlayShoot(bool reverse = false)
    {
        _audioSource.PlayOneShot(reverse ? shootR : shoot); 
    }
    
    public void PlayBulletHit(bool reverse = false)
    {
        _audioSource.PlayOneShot(reverse ? bulletHitR : bulletHit);
    }

    public void PlayDoorOpen()
    {
        _audioSource.PlayOneShot(doorOpen);
    }
    
    public void PlayDoorClose()
    {
        _audioSource.PlayOneShot(doorClose);
    }
    
    public void PlayDoorInsert()
    {
        _audioSource.PlayOneShot(insert);
    }
}