using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerMusic : MonoBehaviour
{
    [SerializeField] private AudioSource backmusic;

    public void vloumeMusic(float value){
        backmusic.volume = value;
    }
}
