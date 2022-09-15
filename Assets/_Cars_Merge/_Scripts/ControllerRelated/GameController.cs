using System;
using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using _Cars_Merge._Scripts.ElementRelated;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Cars_Merge._Scripts.ControllerRelated
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        public bool ambulanceLevel;

        List<Transform> arrows;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Vibration.Init();
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
            arrows = FloorEffectElement.instance.arrows;
            if(newState==GameState.Levelwin)
            {
                for (int i = 0; i < arrows.Count; i++)
                {
                    arrows[i].DOScale(Vector3.zero, 1f);
                    Vector3 arrowRot = arrows[i].transform.eulerAngles;
                    arrows[i].DORotate(new Vector3(arrowRot.x, arrowRot.y + 90, arrowRot.z), 1f).OnComplete(() =>
                    {
                        arrows[i].gameObject.SetActive(false);
                    });
                }
            }
        }

        private void Update()
        {
             if(Input.GetMouseButtonDown(1))
             {
                 On_RetryButtonClicked();
             }
        }

        public void On_NextButtonClicked()
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
            Vibration.Vibrate(27);
        }

        public void On_RetryButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Vibration.Vibrate(27);
        }
        
    }   
}
