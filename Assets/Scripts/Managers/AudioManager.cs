using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);
    }

    private void Update()
    {
        if (!playBgm)
            StopAllBGM();
        else if (!bgm[bgmIndex].isPlaying)
            PlayBGM(bgmIndex);
    }

    public void PlaySFX(int idx, Transform source)
    {
        if (sfx[idx].isPlaying)
            return;

        if (source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, source.position) > sfxMinDistance)
            return;

        if (idx >= 0 && idx < sfx.Length)
        {
            sfx[idx].pitch = Random.Range(.85f, 1.1f);
            sfx[idx].Play();
        }
    }

    public void StopSFX(int idx)
    {
        if (idx >= 0 && idx < sfx.Length)
            sfx[idx].Stop();
    }

    public void PlayBGM(int idx)
    {
        if (idx >= 0 && idx < bgm.Length)
        {
            bgmIndex = idx;
            StopAllBGM();
            bgm[bgmIndex].Play();
        }
    }

    public void PlayRandomBGM()
    {
        PlayBGM(Random.Range(0, bgm.Length));
    }

    private void StopAllBGM()
    {
        foreach (AudioSource audio in bgm)
            audio.Stop();
    }

}
