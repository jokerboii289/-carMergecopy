using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = System.Random;
using UnityEngine.SceneManagement;

namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;

        public GameObject HUD;
        public GameObject winPanel, failPanel;
        public TextMeshProUGUI levelNumText;
        public GameObject winConfetti;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //levelNumText.text = "Lv. " + PlayerPrefs.GetInt("levelnumber", 1);
        }

        private void OnEnable()
        {
            MainController.GameStateChanged += GameManager_GameStateChanged;
        }
        private void OnDisable()
        {
            MainController.GameStateChanged -= GameManager_GameStateChanged;
        }
        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if(newState==GameState.Levelwin)
            {
                StartCoroutine(LevelWon());
            }

            if (newState == GameState.Levelfail)
            {
                HUD.SetActive(false);
                failPanel.SetActive(true);
                SoundsController.instance.PlaySound(SoundsController.instance.fail);
            }
        }

        IEnumerator LevelWon()
        {
            yield return new WaitForSeconds(2.5f);
            HUD.SetActive(false);
            winConfetti.SetActive(true);
            SoundsController.instance.PlaySound(SoundsController.instance.confetti);
            yield return new WaitForSeconds(1.5f);
            winPanel.SetActive(true);
            SoundsController.instance.PlaySound(SoundsController.instance.win);
            Invoke("NextLevel",1);
        }

        void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }  
}
