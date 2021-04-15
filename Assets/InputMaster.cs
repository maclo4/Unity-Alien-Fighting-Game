// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""8c668249-0906-4262-92c5-41703021d2d7"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a3c323a5-ace9-4c53-b533-bd9f34b3f034"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""bc7c6dfb-ce69-4433-bd94-6dc6883eea97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Joystick"",
                    ""type"": ""Value"",
                    ""id"": ""94f6155d-112d-4ab6-8017-454d2462327f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Airdash"",
                    ""type"": ""Button"",
                    ""id"": ""00dc65b3-cbdc-4383-a9a9-d474b1b492a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Light"",
                    ""type"": ""Button"",
                    ""id"": ""24ef5258-8fe0-416f-b312-6ff90ea28810"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Medium Attack"",
                    ""type"": ""Button"",
                    ""id"": ""389dd5d2-4152-4892-a65a-30c14921bbe8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1f92b8b9-9982-4884-81ef-f50eaf6fccf4"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1231d7e9-5207-4082-97cd-89bcec6a1662"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9be5a46-e6c0-4757-89a2-be2462865963"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36bebda0-eec7-44dc-b66c-010c95780ffe"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98c214c3-5ea7-4beb-b540-eb90e43006f1"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ed28d6f-be1d-4499-9924-95d00ed809d3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Airdash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cdf3a45b-caab-4230-b6be-c1958b93cfda"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Light"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8acfa2dd-2244-428b-959e-b94b3fbd87a1"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Medium Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControls_Start = m_PlayerControls.FindAction("Start", throwIfNotFound: true);
        m_PlayerControls_Joystick = m_PlayerControls.FindAction("Joystick", throwIfNotFound: true);
        m_PlayerControls_Airdash = m_PlayerControls.FindAction("Airdash", throwIfNotFound: true);
        m_PlayerControls_Light = m_PlayerControls.FindAction("Light", throwIfNotFound: true);
        m_PlayerControls_MediumAttack = m_PlayerControls.FindAction("Medium Attack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Jump;
    private readonly InputAction m_PlayerControls_Start;
    private readonly InputAction m_PlayerControls_Joystick;
    private readonly InputAction m_PlayerControls_Airdash;
    private readonly InputAction m_PlayerControls_Light;
    private readonly InputAction m_PlayerControls_MediumAttack;
    public struct PlayerControlsActions
    {
        private @InputMaster m_Wrapper;
        public PlayerControlsActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
        public InputAction @Start => m_Wrapper.m_PlayerControls_Start;
        public InputAction @Joystick => m_Wrapper.m_PlayerControls_Joystick;
        public InputAction @Airdash => m_Wrapper.m_PlayerControls_Airdash;
        public InputAction @Light => m_Wrapper.m_PlayerControls_Light;
        public InputAction @MediumAttack => m_Wrapper.m_PlayerControls_MediumAttack;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Start.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnStart;
                @Joystick.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJoystick;
                @Joystick.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJoystick;
                @Joystick.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJoystick;
                @Airdash.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAirdash;
                @Airdash.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAirdash;
                @Airdash.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAirdash;
                @Light.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLight;
                @Light.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLight;
                @Light.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLight;
                @MediumAttack.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMediumAttack;
                @MediumAttack.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMediumAttack;
                @MediumAttack.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMediumAttack;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Joystick.started += instance.OnJoystick;
                @Joystick.performed += instance.OnJoystick;
                @Joystick.canceled += instance.OnJoystick;
                @Airdash.started += instance.OnAirdash;
                @Airdash.performed += instance.OnAirdash;
                @Airdash.canceled += instance.OnAirdash;
                @Light.started += instance.OnLight;
                @Light.performed += instance.OnLight;
                @Light.canceled += instance.OnLight;
                @MediumAttack.started += instance.OnMediumAttack;
                @MediumAttack.performed += instance.OnMediumAttack;
                @MediumAttack.canceled += instance.OnMediumAttack;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    public interface IPlayerControlsActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnJoystick(InputAction.CallbackContext context);
        void OnAirdash(InputAction.CallbackContext context);
        void OnLight(InputAction.CallbackContext context);
        void OnMediumAttack(InputAction.CallbackContext context);
    }
}
