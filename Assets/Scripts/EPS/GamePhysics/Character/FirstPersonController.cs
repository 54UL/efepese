using System;
using EPS.GamePhysics.Core;

using UnityEngine;
using UnityEngine.Serialization;

namespace EPS.GamePhysics.Character
{
	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : InputControllerBase
	{
		[Header("EPS data")]
		public bool IsClient;
		public NetworkPlayer currentNetworkPlayer;
		public Vector3 characterMovement;
		public Vector3 characterRotation;
		public Quaternion aimOrentation;
		public Vector3 playerInputDirection;

		[Header("Player")] [Tooltip("Crouch speed of the character in m/s")]
		public float crouchSpeed = 1.5f;
		[FormerlySerializedAs("MoveSpeed")] [Tooltip("Move speed of the character in m/s")]
		public float moveSpeed = 2.5f;
		[FormerlySerializedAs("SprintSpeed")] [Tooltip("Sprint speed of the character in m/s")]
		public float sprintSpeed = 6.0f;
		[FormerlySerializedAs("RotationSpeed")] [Tooltip("Rotation speed of the character")]
		public float rotationSpeed = 1.0f;
		[FormerlySerializedAs("SpeedChangeRate")] [Tooltip("Acceleration and deceleration")]
		public float speedChangeRate = 10.0f;

		[FormerlySerializedAs("JumpHeight")]
		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float jumpHeight = 1.2f;
		[FormerlySerializedAs("Gravity")] [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float gravity = -15.0f;

		[FormerlySerializedAs("JumpTimeout")]
		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float jumpTimeout = 0.1f;
		[FormerlySerializedAs("FallTimeout")] [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float fallTimeout = 0.15f;

		[FormerlySerializedAs("Grounded")]
		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool grounded = true;
		[FormerlySerializedAs("GroundedOffset")] [Tooltip("Useful for rough ground")]
		public float groundedOffset = -0.14f;
		[FormerlySerializedAs("GroundedRadius")] [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float groundedRadius = 0.5f;
		[FormerlySerializedAs("GroundLayers")] [Tooltip("What layers the character uses as ground")]
		public LayerMask groundLayers;


		[FormerlySerializedAs("TopClamp")]
		[Tooltip("How far in degrees can you move the camera up")]
		public float topClamp = 90.0f;
		[FormerlySerializedAs("BottomClamp")]
		[Tooltip("How far in degrees can you move the camera down")]
		public float bottomClamp = -90.0f;

		// player
		private float _playerCameraTargetPitch;
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout delta time
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		private CharacterController _controller;
		private const float Threshold = 0.01f;
		
		private void Awake()
		{
			
		}

		protected override void Start()
		{
			base.Start();
			_controller = GetComponent<CharacterController>();
			

			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			
			currentNetworkPlayer.SendInputs(characterMovement, characterRotation, aimOrentation, playerInputDirection);
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			var position = transform.position;
			Vector3 spherePosition = new Vector3(position.x, position.y - groundedOffset, position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (!(_inputActions.look.sqrMagnitude >= Threshold)) {
				characterRotation = Vector2.zero;
				return;
			}
			//Don't multiply mouse input by Time.deltaTime
			float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

			_playerCameraTargetPitch += _inputActions.look.y * rotationSpeed * deltaTimeMultiplier;
			_rotationVelocity = _inputActions.look.x * rotationSpeed * deltaTimeMultiplier;

			// clamp our pitch rotation
			_playerCameraTargetPitch = ClampAngle(_playerCameraTargetPitch, bottomClamp, topClamp);

			// Update Cinemachine camera target pitch
			aimOrentation = Quaternion.Euler(_playerCameraTargetPitch, 0.0f, 0.0f); 

		 //TODO: MOVER AL NETWORK PLAYER
		 //PlayerCamera.transform.localRotation = Quaternion.Euler(_playerCameraTargetPitch, 0.0f, 0.0f);

			characterRotation = Vector3.up * _rotationVelocity;

			//TODO: MOVER AL NETWORK PLAYER
			//transform.Rotate(Vector3.up * _rotationVelocity); 
		}

		private void Move()
		{
			
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _inputActions.sprint ? sprintSpeed : moveSpeed;

			// set target speed to move speed when walking sideways
			double tolerance = 0.001;
			if (Math.Abs(_inputActions.move.x + 1) < tolerance || Math.Abs(_inputActions.move.x - 1) < tolerance) targetSpeed = moveSpeed;

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_inputActions.move == Vector2.zero) targetSpeed = 0.0f;


			// a reference to the players current horizontal velocity
			var velocity = _controller.velocity;
			float currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _inputActions.analogMovement ? _inputActions.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_inputActions.move.x, 0.0f, _inputActions.move.y).normalized;
			
			// animate the player 
			var transformProp = transform;
			var playerVelocity = transformProp.rotation * new Vector3(-velocity.x, 0.0f, velocity.z);
            
			playerInputDirection = playerVelocity;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_inputActions.move != Vector2.zero)
			{
				// move
				inputDirection = transformProp.right * _inputActions.move.x + transformProp.forward * _inputActions.move.y;
			}

			// move the player
			characterMovement = (inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = fallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_inputActions.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_inputActions.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			Gizmos.color = grounded ? transparentGreen : transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			var position = transform.position;
			Gizmos.DrawSphere(new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}
	}
}