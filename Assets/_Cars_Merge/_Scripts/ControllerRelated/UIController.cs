using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


namespace _Draw_Copy._Scripts.ControllerRelated
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;

        public GameObject HUD;
        public GameObject winPanel, failPanel;
        public TextMeshProUGUI levelNumText;
        public GameObject winConfetti;
        [Header("BG music")]
        public GameObject bgMusic;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            levelNumText.text = "Lv. " + PlayerPrefs.GetInt("levelnumber", 1);
            bgMusic.SetActive(true);
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
                //failPanel.SetActive(true);
                SoundsController.instance.PlaySound(SoundsController.instance.fail);
                bgMusic.SetActive(false);
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
            bgMusic.SetActive(false);
        }

        public void Nextlvl()
        {
            if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(UnityEngine.Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
            }
            PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);

        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }  
}
