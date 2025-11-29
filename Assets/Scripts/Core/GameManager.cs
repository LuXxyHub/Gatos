using UnityEngine;
using System;

namespace CosmicYarnCat.Core
{
    public enum GameState
    {
        Exploration,
        Combat,
        Paused,
        Dialogue
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; }

        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetState(GameState.Exploration);
        }

        public void SetState(GameState newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(newState);
            Debug.Log($"Game State Changed to: {newState}");
        }

        public void PauseGame()
        {
            if (CurrentState != GameState.Paused)
            {
                Time.timeScale = 0f;
                SetState(GameState.Paused);
            }
            else
            {
                Time.timeScale = 1f;
                SetState(GameState.Exploration); // Or previous state
            }
        }
    }
}
