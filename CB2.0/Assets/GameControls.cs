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
                    ""name"": ""pick/drop"",
                    ""type"": ""Button"",
                    ""id"": ""f6cab9d2-190f-47ae-8309-1821e4ee654f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""use"",
                    ""type"": ""Button"",
                    ""id"": ""a4e3b9ad-8089-4e85-b30f-6ce37700d5f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""dash"",
                    ""type"": ""Button"",
                    ""id"": ""097a889b-ad83-4223-936e-1ee3cd934b16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4e24c216-918f-4eee-9298-eb1a19895cb9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""shop"",
                    ""type"": ""Button"",
                    ""id"": ""e244ebdd-9f7d-4329-9dde-bd3c459f709e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""show"",
                    ""type"": ""Button"",
                    ""id"": ""f0f653bb-5b5d-48a9-bc1f-9b39f81ba57f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7ba06224-ab37-4eb7-9637-de9516ad30ca"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""shop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4dc1c433-c18f-4556-8bcd-ab414ecdf290"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""shop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14944c21-3160-47af-b019-9301f58de6d2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""43e5feb8-3878-4447-b8a3-d1aa82716725"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4f76f6e4-53da-478a-9553-1bc4947ade44"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3d9f6054-f2d9-46d3-af37-963571f7e0a7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""93bbb2e8-337b-4f58-a6e8-d85b299af619"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d820ebe7-ef77-40e2-8ec9-73f3357878aa"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""af0f93e1-e40f-48be-b2b7-bac158b0ceb9"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80eb328a-a238-4454-b61d-f4445113ab99"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66669358-f8d5-4c1c-9134-dfb5e23aacb0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bc4ded6-f994-4d8c-8315-fd739aeb54d8"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22233c4d-25c7-42a1-9c99-29ff2136e8f4"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0e5ff73-5666-4f2c-aadd-6682ff843170"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""pick/drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40a62a76-ce50-444a-a6e2-5af3920ed193"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""pick/drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50a9945a-f015-4e62-a894-87e33019ee7e"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard"",
                    ""action"": ""show"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b70fd99-8a6f-4b0e-8dd1-d5173e5497b8"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player 1;Player 2;Player 3;Player 4"",
                    ""action"": ""show"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player 1"",
            ""bindingGroup"": ""Player 1"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""keyboard"",
            ""bindingGroup"": ""keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Player 2"",
            ""bindingGroup"": ""Player 2"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Player 3"",
            ""bindingGroup"": ""Player 3"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Player 4"",
            ""bindingGroup"": ""Player 4"",
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
        m_gameplay_pickdrop = m_gameplay.FindAction("pick/drop", throwIfNotFound: true);
        m_gameplay_use = m_gameplay.FindAction("use", throwIfNotFound: true);
        m_gameplay_dash = m_gameplay.FindAction("dash", throwIfNotFound: true);
        m_gameplay_move = m_gameplay.FindAction("move", throwIfNotFound: true);
        m_gameplay_shop = m_gameplay.FindAction("shop", throwIfNotFound: true);
        m_gameplay_show = m_gameplay.FindAction("show", throwIfNotFound: true);
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
    private readonly InputAction m_gameplay_pickdrop;
    private readonly InputAction m_gameplay_use;
    private readonly InputAction m_gameplay_dash;
    private readonly InputAction m_gameplay_move;
    private readonly InputAction m_gameplay_shop;
    private readonly InputAction m_gameplay_show;
    public struct GameplayActions
    {
        private @GameControls m_Wrapper;
        public GameplayActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @pickdrop => m_Wrapper.m_gameplay_pickdrop;
        public InputAction @use => m_Wrapper.m_gameplay_use;
        public InputAction @dash => m_Wrapper.m_gameplay_dash;
        public InputAction @move => m_Wrapper.m_gameplay_move;
        public InputAction @shop => m_Wrapper.m_gameplay_shop;
        public InputAction @show => m_Wrapper.m_gameplay_show;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @pickdrop.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPickdrop;
                @pickdrop.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPickdrop;
                @pickdrop.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPickdrop;
                @use.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUse;
                @use.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUse;
                @use.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUse;
                @dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @shop.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShop;
                @shop.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShop;
                @shop.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShop;
                @show.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShow;
                @show.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShow;
                @show.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShow;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @pickdrop.started += instance.OnPickdrop;
                @pickdrop.performed += instance.OnPickdrop;
                @pickdrop.canceled += instance.OnPickdrop;
                @use.started += instance.OnUse;
                @use.performed += instance.OnUse;
                @use.canceled += instance.OnUse;
                @dash.started += instance.OnDash;
                @dash.performed += instance.OnDash;
                @dash.canceled += instance.OnDash;
                @move.started += instance.OnMove;
                @move.performed += instance.OnMove;
                @move.canceled += instance.OnMove;
                @shop.started += instance.OnShop;
                @shop.performed += instance.OnShop;
                @shop.canceled += instance.OnShop;
                @show.started += instance.OnShow;
                @show.performed += instance.OnShow;
                @show.canceled += instance.OnShow;
            }
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    private int m_Player1SchemeIndex = -1;
    public InputControlScheme Player1Scheme
    {
        get
        {
            if (m_Player1SchemeIndex == -1) m_Player1SchemeIndex = asset.FindControlSchemeIndex("Player 1");
            return asset.controlSchemes[m_Player1SchemeIndex];
        }
    }
    private int m_keyboardSchemeIndex = -1;
    public InputControlScheme keyboardScheme
    {
        get
        {
            if (m_keyboardSchemeIndex == -1) m_keyboardSchemeIndex = asset.FindControlSchemeIndex("keyboard");
            return asset.controlSchemes[m_keyboardSchemeIndex];
        }
    }
    private int m_Player2SchemeIndex = -1;
    public InputControlScheme Player2Scheme
    {
        get
        {
            if (m_Player2SchemeIndex == -1) m_Player2SchemeIndex = asset.FindControlSchemeIndex("Player 2");
            return asset.controlSchemes[m_Player2SchemeIndex];
        }
    }
    private int m_Player3SchemeIndex = -1;
    public InputControlScheme Player3Scheme
    {
        get
        {
            if (m_Player3SchemeIndex == -1) m_Player3SchemeIndex = asset.FindControlSchemeIndex("Player 3");
            return asset.controlSchemes[m_Player3SchemeIndex];
        }
    }
    private int m_Player4SchemeIndex = -1;
    public InputControlScheme Player4Scheme
    {
        get
        {
            if (m_Player4SchemeIndex == -1) m_Player4SchemeIndex = asset.FindControlSchemeIndex("Player 4");
            return asset.controlSchemes[m_Player4SchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnPickdrop(InputAction.CallbackContext context);
        void OnUse(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnShop(InputAction.CallbackContext context);
        void OnShow(InputAction.CallbackContext context);
    }
}
