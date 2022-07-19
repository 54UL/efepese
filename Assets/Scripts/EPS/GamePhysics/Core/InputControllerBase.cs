
using EPS.Core.Services.Implementations;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EPS.GamePhysics.Core
{
    public class InputControllerBase : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputService _inputService;
        protected PlayerActions _inputActions;

        protected bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";
        
        protected virtual void Start()
        {
            _inputService = ServiceInjector.getSingleton<InputService>();
            _inputActions = _inputService.GetInputActions();
            _playerInput = _inputService.GetPlayerInput();
        }
    }
}
