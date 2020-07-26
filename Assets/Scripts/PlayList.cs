using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayList : MonoBehaviour
{
    [SerializeField] AudioClip[] myMusic;

    AudioSource music;
    private int randomMusic;
    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        RandomMusicPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!music.isPlaying)
        {
            RandomMusicPlay();
        }
    }
    private void RandomMusicPlay()
    {
        randomMusic = Random.Range(0, myMusic.Length);
        music.clip = myMusic[randomMusic];
        music.Play();
    }
}
