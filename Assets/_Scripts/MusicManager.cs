using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager INS;
    private AudioSource audioSource;
    public bool seTocoUnaVez;

    private void Awake()
    {
        if(INS == null)
        {
            INS = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        ActivarDesactivarMusica();
    }

    private void ActivarDesactivarMusica()
    {
        if(seTocoUnaVez) 
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = 1;
        }
    }
}
