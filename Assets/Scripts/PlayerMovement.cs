using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private PlayerWeapon _playerWeapon;
    private Vector2 _moveInput;
    private Vector3 _mouseDirection;


    private void Start()
    {
        if (_playerStats == null)
        {
            _playerStats = GetComponent<PlayerStats>();
        }
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        if (_playerAnimator == null)
        {
            _playerAnimator = _playerVisual.GetComponent<Animator>();
        }
        if (_playerWeapon == null)
        {
            _playerWeapon = GetComponentInChildren<PlayerWeapon>();
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        //// Directions caméra
        //Vector3 camForward = _mainCamera.transform.forward;
        //Vector3 camRight = _mainCamera.transform.right;

        //// On annule l'influence verticale
        //camForward.y = 0f;
        //camRight.y = 0f;

        //camForward.Normalize();
        //camRight.Normalize();

        //Vector3 move =
        //    camForward * _moveInput.y +
        //    camRight * _moveInput.x;

        //transform.Translate(
        //    move * Time.deltaTime * _playerStats.GetMoveSpeed(),
        //    Space.World
        //    );
        ////Vector3 move = new Vector3(_moveInput.x, 0f, _moveInput.y);
        //transform.Translate(
        //    move * Time.deltaTime * _playerStats.GetMoveSpeed(),
        //    Space.World
        //);

        // On récupère UNIQUEMENT la rotation Y de la caméra
        Vector3 forward = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 right = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * Vector3.right;

        Vector3 move =
            forward * _moveInput.y +
            right * _moveInput.x;

        transform.Translate(
            move * Time.deltaTime * _playerStats.GetMoveSpeed(),
            Space.World
        );
    }


    private void HandleRotation()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 lookDir = hit.point - _playerVisual.position;
            lookDir.y = 0f;

            if (lookDir.sqrMagnitude > 0.01f)
            {
                _playerVisual.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }

    private void UpdateAnimator()
    {
        Vector3 worldMove = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (worldMove.sqrMagnitude < 0.001f)
        {
            _playerAnimator.SetFloat("MoveX", 0f);
            _playerAnimator.SetFloat("MoveY", 0f);
            _playerAnimator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 localMove = _playerVisual.InverseTransformDirection(worldMove.normalized);

        _playerAnimator.SetFloat("MoveX", localMove.x);
        _playerAnimator.SetFloat("MoveY", localMove.z);
        _playerAnimator.SetFloat("Speed", worldMove.magnitude);
    }


    private void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimator();
    }

    public void AttackAnimation()
    {
        WeaponStats weaponStats = _playerWeapon.GetCurrentWeaponStats();
        if (_playerWeapon == null) return;
        if (weaponStats.IsRanged())
            _playerAnimator.Play("Attack01", 0, 0f);
        if (!weaponStats.IsRanged())
            _playerAnimator.Play("Attack04", 0, 0f);

    }
}
