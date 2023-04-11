
using EPS.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using InputSystem;

namespace EPS.GamePhysics.Core
{
    public class InputControllerBase : MonoBehaviour
    {
        public PlayerInput _playerInput;
        public InputService _inputService;
        public PlayerActions _inputActions;

        protected bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";
        
        protected virtual void Start()
        {
            _inputService = ServiceInjector.getSingleton<InputService>();
            _inputActions = _inputService.GetInputActions();
            _playerInput = _inputService.GetPlayerInput();
        }
    }
}
