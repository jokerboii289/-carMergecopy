using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace _Cars_Merge._Scripts.ControllerRelated
{
    public enum GameState
    {
        None,
        Create,
        Input,
        Movement,
        Levelwin,
        Levelfail
    }

    public class MainController : MonoBehaviour
    {
        public static MainController instance;
        
        [SerializeField] private GameState _gameState;
        public static event Action<GameState, GameState> GameStateChanged;

        public GameState GameState
        {
            get => _gameState;
            private set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;
                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
            }
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            CreateGame();
        }

        void CreateGame()
        {
            GameState = GameState.Create;
            DOVirtual.DelayedCall(1.85f, () =>
            {
                GameState = GameState.Input;
            });
        }

        public void SetActionType(GameState _curState)
        {
            GameState = _curState;
        }
    }
}