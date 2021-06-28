// GENERATED AUTOMATICALLY FROM 'Assets/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""15e7ee15-ffb2-4636-ad3a-1c1d753eaa22"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8613cedc-1f43-4fd2-8131-458e1dcc02e2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""dash"",
                    ""type"": ""Button"",
                    ""id"": ""f6c0006d-4f97-4efe-bc8e-608952c258c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""use"",
                    ""type"": ""Button"",
                    ""id"": ""8b30095e-8a3c-49f5-be8d-231d64a8ce01"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""pick/drop"",
                    ""type"": ""Button"",
                    ""id"": ""92ef2e6a-f46a-4ab2-9be3-5ccb075cbebb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""shop"",
                    ""type"": ""Button"",
                    ""id"": ""0b7017e6-3db1-423b-a55b-ae969b53d812"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cc238556-64e7-4ae8-b639-2e90a5431056"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""controller"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a5310a5-7ee3-4931-89fc-e65de2e6be90"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""controller"",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0d28a67-2b12-4eb5-b9d5-4a3927883eb2"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""controller"",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea195df4-2602-44c9-8c89-49c988eb59e3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""controller"",
                    ""action"": ""use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1b36df1-297e-4843-ae04-aa005a99ac5f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""controller"",
                    ""action"": ""pick/drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""582b3c19-7f7a-41f1-bc42-5fc81dbcd5e0"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""controller"",
                    ""action"": ""shop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""controller"",
            ""bindingGroup"": ""controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_move = m_gameplay.FindAction("move", throwIfNotFound: true);
        m_gameplay_dash = m_gameplay.FindAction("dash", throwIfNotFound: true);
        m_gameplay_use = m_gameplay.FindAction("use", throwIfNotFound: true);
        m_gameplay_pickdrop = m_gameplay.FindAction("pick/drop", throwIfNotFound: true);
        m_gameplay_shop = m_gameplay.FindAction("shop", throwIfNotFound: true);
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

    // gameplay
    private readonly InputActionMap m_gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_gameplay_move;
    private readonly InputAction m_gameplay_dash;
    private readonly InputAction m_gameplay_use;
    private readonly InputAction m_gameplay_pickdrop;
    private readonly InputAction m_gameplay_shop;
    public struct GameplayActions
    {
        private @GameControls m_Wrapper;
        public GameplayActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @move => m_Wrapper.m_gameplay_move;
        public InputAction @dash => m_Wrapper.m_gameplay_dash;
        public InputAction @use => m_Wrapper.m_gameplay_use;
        public InputAction @pickdrop => m_Wrapper.m_gameplay_pickdrop;
        public InputAction @shop => m_Wrapper.m_gameplay_shop;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @use.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUse;
                @use.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUse;
                @use.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUse;
                @pickdrop.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPickdrop;
                @pickdrop.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPickdrop;
                @pickdrop.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPickdrop;
                @shop.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShop;
                @shop.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShop;
                @shop.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShop;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @move.started += instance.OnMove;
                @move.performed += instance.OnMove;
                @move.canceled += instance.OnMove;
                @dash.started += instance.OnDash;
                @dash.performed += instance.OnDash;
                @dash.canceled += instance.OnDash;
                @use.started += instance.OnUse;
                @use.performed += instance.OnUse;
                @use.canceled += instance.OnUse;
                @pickdrop.started += instance.OnPickdrop;
                @pickdrop.performed += instance.OnPickdrop;
                @pickdrop.canceled += instance.OnPickdrop;
                @shop.started += instance.OnShop;
                @shop.performed += instance.OnShop;
                @shop.canceled += instance.OnShop;
            }
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    private int m_controllerSchemeIndex = -1;
    public InputControlScheme controllerScheme
    {
        get
        {
            if (m_controllerSchemeIndex == -1) m_controllerSchemeIndex = asset.FindControlSchemeIndex("controller");
            return asset.controlSchemes[m_controllerSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
        void OnPickdrop(InputAction.CallbackContext context);
        void OnShop(InputAction.CallbackContext context);
    }
}
