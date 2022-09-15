using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class SoundsController : MonoBehaviour
    {
        public static SoundsController instance;

        public AudioSource mainAudioSource;
        public AudioClip merge, crash,confetti, win, fail, arrowTap;

        private void Awake()
        {
            instance = this;
        }

        public void PlaySound(AudioClip clip)
        {
            mainAudioSource.PlayOneShot(clip);
        }
    }   
}
