using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace JBV
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        [SerializeField] private Vector2 movementInput;

        private PlayerControls playerControls;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            Instance.enabled = false;
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += ctx =>
                {
                    movementInput = ctx.ReadValue<Vector2>();
                };
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;

            playerControls.Dispose();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            Instance.enabled = newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex();
        }
    }
}
