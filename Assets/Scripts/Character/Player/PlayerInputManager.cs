using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace JBV
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        [Header("Movement Input")]
        [SerializeField] private Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("Camera Movement Input")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

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

                playerControls.PlayerCamera.Movement.performed += ctx =>
                {
                    cameraInput = ctx.ReadValue<Vector2>();
                };
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;

            playerControls.Dispose();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!enabled)
            {
                return;
            }

            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }

        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            bool isWorldScene = newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex();

            Instance.enabled = isWorldScene;

            Cursor.visible = isWorldScene;
            Cursor.lockState = isWorldScene ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5f && moveAmount > 0f)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1f)
            {
                moveAmount = 1f;
            }
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }
}
