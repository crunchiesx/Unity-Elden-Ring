using UnityEngine;
using UnityEngine.EventSystems;

namespace JBV
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        [SerializeField] private float walkingSpeed = 2f;
        [SerializeField] private float runningSpeed = 5f;
        [SerializeField] private float rotationSpeed = 15f;

        private Vector3 movementDirection;
        private Vector3 targetRotationDirection;

        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            GetVerticalAndHorizontalInputs();

            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            movementDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            movementDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            movementDirection.Normalize();
            movementDirection.y = 0f;

            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                player.characterController.Move(runningSpeed * Time.deltaTime * movementDirection);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f && PlayerInputManager.Instance.moveAmount > 0f)
            {
                player.characterController.Move(Time.deltaTime * walkingSpeed * movementDirection);
            }
        }

        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0f;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}