using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerStateManager : MonoBehaviour, IDataPersistance
{
    // Reference variables
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;
    BatteryHealth _batteryHealth;

    // Animation variables
    int _isWalkingHash;
    int _isRunningHash;
    int _isJumpingHash;
    int _jumpCountHash;
    int _isFallingHash;
    int _shouldDieHash;
    int _shouldAliveHash;

    // Movement variables
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _appliedMovement;
    Vector3 _cameraRelativeMovement;
    bool _isMovementPressed = false;
    bool _isRunPressed = false;
    float _rotationFactorPerFrame = 15;
    float _runMultiplier = 3.0f;

    bool _isDead = false;

    // Gravity variables
    float _gravity = -9.81f;
    float _maxFallVelocity = -50.0f;

    // Jumping variables
    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    bool _requireNewJumpPress = false;
    float _maxJumpHeight = 1;
    float _maxJumpTime = 0.75f;
    //bool _isJumping = false;
    int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;

    // State machine variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    [SerializeField] bool _isOvergrown = false;
    List<ChangeBodyMeshes> _bodyMeshes = new List<ChangeBodyMeshes>();

    // Getters and setters
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public BatteryHealth BatteryHealth { get { return _batteryHealth; } }
    public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
    public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; } }
    public int IsWalkingHash { get { return _isWalkingHash; } }
    public int IsRunningHash { get { return _isRunningHash; } }
    public int IsJumpingHash { get { return _isJumpingHash; } }
    public int IsFallingHash { get { return _isFallingHash; } }
    public int ShouldDieHash { get { return _shouldDieHash; } }
    public int JumpCountHash { get { return _jumpCountHash; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    //public bool IsJumping { set { _isJumping = value; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public float Gravity { get { return _gravity; } }
    public float MaxFallVelocity { get { return _maxFallVelocity; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float RunMultiplier { get { return _runMultiplier; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }

    private void Awake()
    {
        // Initially set reference variables
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _batteryHealth = GetComponentInChildren<BatteryHealth>(true);

        // Setup the state machine
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        // Set the animation hash variables
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");
        _isFallingHash = Animator.StringToHash("isFalling");
        _shouldDieHash = Animator.StringToHash("shouldDie");
        _shouldAliveHash = Animator.StringToHash("shouldAlive");

        // Set the jump variables
        SetUpJumpVariables();
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
        _bodyMeshes.AddRange(GetComponentsInChildren<ChangeBodyMeshes>());
    }

    private void OnEnable()
    {
        // Enable the input system
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Move.performed += OnMove;
        _playerInput.CharacterControls.Move.canceled += OnMove;

        _playerInput.CharacterControls.Run.performed += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;

        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;


    }

    private void OnDisable()
    {
        // Disable the input system
        _playerInput.CharacterControls.Disable();
        _playerInput.CharacterControls.Move.performed -= OnMove;
        _playerInput.CharacterControls.Move.canceled -= OnMove;

        _playerInput.CharacterControls.Run.performed -= OnRun;
        _playerInput.CharacterControls.Run.canceled -= OnRun;

        _playerInput.CharacterControls.Jump.started -= OnJump;
        _playerInput.CharacterControls.Jump.canceled -= OnJump;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.StateFactory.Playing())
        {
            foreach (ChangeBodyMeshes bodyMesh in _bodyMeshes)
            {
                bodyMesh.ChangeMesh(_isOvergrown);
            }

            //Debug.Log(CharacterController.isGrounded);
            HandleRotation();
            _currentState.UpdateStates();
            _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
            _characterController.Move(_cameraRelativeMovement * Time.deltaTime);
        }
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.StateFactory.Playing())
        {
            // Read the value of the input action
            _currentMovementInput = ctx.ReadValue<Vector2>();
            _currentMovement.x = _currentMovementInput.x;
            _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;

            _currentMovement.z = _currentMovementInput.y;
            _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        }
    }

    void OnRun(InputAction.CallbackContext ctx)
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.StateFactory.Playing())
        {
            _isRunPressed = ctx.ReadValueAsButton();
        }
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.StateFactory.Playing())
        {
            _isJumpPressed = ctx.ReadValueAsButton();
            _requireNewJumpPress = false;
        }
    }

    void SetUpJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        float initialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;

        float secondJumpGravity = (-2 * (_maxJumpHeight + 1)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (_maxJumpHeight + 1)) / (timeToApex * 1.25f);

        float thirdJumpGravity = (-2 * (_maxJumpHeight + 1.5f)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (_maxJumpHeight + 1.5f)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, _initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        _jumpGravities.Add(0, initialGravity);
        _jumpGravities.Add(1, initialGravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        // Set the position to look at based on the current movement input
        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _cameraRelativeMovement.z;

        // Current rotation of the player
        Quaternion currentRotation = transform.rotation;

        // If the player is moving, rotate the player to face the direction of movement
        if (_isMovementPressed)
        {
            // Creates a new rotation based on the position to look at
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            //Debug.Log($"Rotation: {targetRotation}");
            // Slerp the current rotation to the target rotation
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }

    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        // Get the current y value of the vector
        float currentYValue = vectorToRotate.y;

        // Get the camera's forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // set the y values to 0 to ignore the y axis
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the vectors
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    Coroutine _resetHealthCoroutine;
    void TakeBatteryHealth(string Parameters)
    {
        if (_batteryHealth.enabled)
        {
            string[] ParametersList = Parameters.Split(",");
            int min = int.Parse(ParametersList[0]);
            int max = int.Parse(ParametersList[1]);
            int threshold = int.Parse(ParametersList[2]);
            int damage = int.Parse(ParametersList[3]);

            if (_resetHealthCoroutine == null)
            {
                int genNum = Random.Range(min, max);

                if (genNum <= threshold)
                {
                    BatteryHealth.GetComponent<BatteryHealth>().DecreaseHealth(damage);
                    _resetHealthCoroutine = StartCoroutine(ResetTakeBatteryHealthTimer());
                }
            }
        }
    }

    IEnumerator ResetTakeBatteryHealthTimer()
    {
        yield return new WaitForSeconds(30);
        _resetHealthCoroutine = null;
    }

    public void Alive()
    {
        _animator.SetTrigger(_shouldAliveHash);
        _isDead = false;
    }

    public void Dead()
    {
        _isDead = true;
        _currentState = _states.Dead();
        _currentState.EnterState();
    }

    public void LoadData(GameData gameData)
    {
        CharacterController.enabled = false;
        CharacterController.transform.position = gameData.startLocation;
        CharacterController.transform.rotation = gameData.startRotation;
        CharacterController.enabled = true;
        _resetHealthCoroutine = null;
        _animator.SetBool(_isJumpingHash, false);
        _animator.SetBool(_isRunningHash, false);
        _animator.SetBool(_isWalkingHash, false);
        _animator.SetBool(_isFallingHash, false);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.startLocation = transform.position;
        gameData.startRotation = transform.rotation;
        gameData.dateLastSaved = System.DateTime.Now.ToString();
    }
}
