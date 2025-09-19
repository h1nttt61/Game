using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

/// <summary>
/// ��������� �����, ������ ������ �� �����
/// </summary>
public class GameInput : MonoBehaviour
{
    /// <summary>
    /// �������� static - ��������� � ������
    /// </summary>
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;

    public event EventHandler OnPlayerDash;

    /// <summary>
    /// ���������� ������� � Unity , ����������� �� Start
    /// </summary>
    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        _playerInputActions.Combat.Attack.started += PlayerAttack_Started; //��� ������� �� ������
        _playerInputActions.Player.Dash.performed += DashPerformed; //����� ��������� ������ performed
    }

    private void DashPerformed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerAttack_Started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 pos = Mouse.current.position.ReadValue();
        return pos;
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }
}
