using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Transform _playerVisual;
    [SerializeField] private Animator _playerAnimator;
    private Vector2 _moveInput;

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        transform.Translate(move * Time.deltaTime * _playerStats.GetMoveSpeed(), Space.World);
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
        _playerAnimator.Play("Attack01", 0, 0f);
    }
}
