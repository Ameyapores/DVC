// GENERATED AUTOMATICALLY FROM 'Assets/Joystick_controller.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Joystick_controller : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Joystick_controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Joystick_controller"",
    ""maps"": [
        {
            ""name"": ""Movements"",
            ""id"": ""16df3fc6-522a-4f3b-bc02-8d15c59452b4"",
            ""actions"": [
                {
                    ""name"": ""Go_forward"",
                    ""type"": ""Value"",
                    ""id"": ""1aef144a-e21d-4f01-a270-5371c22b0bef"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Go_backward"",
                    ""type"": ""Button"",
                    ""id"": ""b2596b6e-6f34-4ae2-a168-726750b292ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Deflect"",
                    ""type"": ""Value"",
                    ""id"": ""9656be69-2e84-4cc9-9308-dda8460c53ea"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Torque_right"",
                    ""type"": ""Value"",
                    ""id"": ""a7dd07a2-c249-40ad-846e-05f479702306"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Torque_left"",
                    ""type"": ""Value"",
                    ""id"": ""9f40ae72-f689-48c0-aa2a-0a7921e55a95"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Activate"",
                    ""type"": ""Button"",
                    ""id"": ""ddffd138-655f-4272-8c10-1d731e2c81f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Capture"",
                    ""type"": ""Button"",
                    ""id"": ""5a5405ca-1644-4245-b40f-dec43cb536f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Speed_up"",
                    ""type"": ""Button"",
                    ""id"": ""f178ba5a-558c-4eda-9fc3-d9343e8c322c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Speed_down"",
                    ""type"": ""Button"",
                    ""id"": ""4e94bcb5-7333-4eaf-b102-9c0c3cafdef7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""86b507ba-44ab-48ce-a690-4da2165612e6"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Go_forward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9542becd-e9ff-4a16-8b35-519a900261f9"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Go_backward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10629ad8-60b2-4792-8dd9-3e5ecb721319"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque_right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b8a2de8-88fb-4805-8b5e-e3379b6e69d7"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Torque_left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f3225c9e-3a58-4f07-9677-d516a9fc88b5"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deflect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d60c027e-992e-4b74-b23f-5ad6712081ea"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Activate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5a9beda-eddb-40a7-a510-cab4def121dc"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Capture"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d22ed128-9ee8-4367-815f-772a6cf1fe27"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Speed_up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ebbeb28-300a-43a5-acc2-5fd2e9c7f83a"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Speed_down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movements
        m_Movements = asset.FindActionMap("Movements", throwIfNotFound: true);
        m_Movements_Go_forward = m_Movements.FindAction("Go_forward", throwIfNotFound: true);
        m_Movements_Go_backward = m_Movements.FindAction("Go_backward", throwIfNotFound: true);
        m_Movements_Deflect = m_Movements.FindAction("Deflect", throwIfNotFound: true);
        m_Movements_Torque_right = m_Movements.FindAction("Torque_right", throwIfNotFound: true);
        m_Movements_Torque_left = m_Movements.FindAction("Torque_left", throwIfNotFound: true);
        m_Movements_Activate = m_Movements.FindAction("Activate", throwIfNotFound: true);
        m_Movements_Capture = m_Movements.FindAction("Capture", throwIfNotFound: true);
        m_Movements_Speed_up = m_Movements.FindAction("Speed_up", throwIfNotFound: true);
        m_Movements_Speed_down = m_Movements.FindAction("Speed_down", throwIfNotFound: true);
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

    // Movements
    private readonly InputActionMap m_Movements;
    private IMovementsActions m_MovementsActionsCallbackInterface;
    private readonly InputAction m_Movements_Go_forward;
    private readonly InputAction m_Movements_Go_backward;
    private readonly InputAction m_Movements_Deflect;
    private readonly InputAction m_Movements_Torque_right;
    private readonly InputAction m_Movements_Torque_left;
    private readonly InputAction m_Movements_Activate;
    private readonly InputAction m_Movements_Capture;
    private readonly InputAction m_Movements_Speed_up;
    private readonly InputAction m_Movements_Speed_down;
    public struct MovementsActions
    {
        private @Joystick_controller m_Wrapper;
        public MovementsActions(@Joystick_controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Go_forward => m_Wrapper.m_Movements_Go_forward;
        public InputAction @Go_backward => m_Wrapper.m_Movements_Go_backward;
        public InputAction @Deflect => m_Wrapper.m_Movements_Deflect;
        public InputAction @Torque_right => m_Wrapper.m_Movements_Torque_right;
        public InputAction @Torque_left => m_Wrapper.m_Movements_Torque_left;
        public InputAction @Activate => m_Wrapper.m_Movements_Activate;
        public InputAction @Capture => m_Wrapper.m_Movements_Capture;
        public InputAction @Speed_up => m_Wrapper.m_Movements_Speed_up;
        public InputAction @Speed_down => m_Wrapper.m_Movements_Speed_down;
        public InputActionMap Get() { return m_Wrapper.m_Movements; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementsActions set) { return set.Get(); }
        public void SetCallbacks(IMovementsActions instance)
        {
            if (m_Wrapper.m_MovementsActionsCallbackInterface != null)
            {
                @Go_forward.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnGo_forward;
                @Go_forward.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnGo_forward;
                @Go_forward.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnGo_forward;
                @Go_backward.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnGo_backward;
                @Go_backward.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnGo_backward;
                @Go_backward.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnGo_backward;
                @Deflect.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnDeflect;
                @Deflect.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnDeflect;
                @Deflect.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnDeflect;
                @Torque_right.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnTorque_right;
                @Torque_right.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnTorque_right;
                @Torque_right.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnTorque_right;
                @Torque_left.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnTorque_left;
                @Torque_left.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnTorque_left;
                @Torque_left.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnTorque_left;
                @Activate.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnActivate;
                @Activate.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnActivate;
                @Activate.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnActivate;
                @Capture.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnCapture;
                @Capture.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnCapture;
                @Capture.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnCapture;
                @Speed_up.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnSpeed_up;
                @Speed_up.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnSpeed_up;
                @Speed_up.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnSpeed_up;
                @Speed_down.started -= m_Wrapper.m_MovementsActionsCallbackInterface.OnSpeed_down;
                @Speed_down.performed -= m_Wrapper.m_MovementsActionsCallbackInterface.OnSpeed_down;
                @Speed_down.canceled -= m_Wrapper.m_MovementsActionsCallbackInterface.OnSpeed_down;
            }
            m_Wrapper.m_MovementsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Go_forward.started += instance.OnGo_forward;
                @Go_forward.performed += instance.OnGo_forward;
                @Go_forward.canceled += instance.OnGo_forward;
                @Go_backward.started += instance.OnGo_backward;
                @Go_backward.performed += instance.OnGo_backward;
                @Go_backward.canceled += instance.OnGo_backward;
                @Deflect.started += instance.OnDeflect;
                @Deflect.performed += instance.OnDeflect;
                @Deflect.canceled += instance.OnDeflect;
                @Torque_right.started += instance.OnTorque_right;
                @Torque_right.performed += instance.OnTorque_right;
                @Torque_right.canceled += instance.OnTorque_right;
                @Torque_left.started += instance.OnTorque_left;
                @Torque_left.performed += instance.OnTorque_left;
                @Torque_left.canceled += instance.OnTorque_left;
                @Activate.started += instance.OnActivate;
                @Activate.performed += instance.OnActivate;
                @Activate.canceled += instance.OnActivate;
                @Capture.started += instance.OnCapture;
                @Capture.performed += instance.OnCapture;
                @Capture.canceled += instance.OnCapture;
                @Speed_up.started += instance.OnSpeed_up;
                @Speed_up.performed += instance.OnSpeed_up;
                @Speed_up.canceled += instance.OnSpeed_up;
                @Speed_down.started += instance.OnSpeed_down;
                @Speed_down.performed += instance.OnSpeed_down;
                @Speed_down.canceled += instance.OnSpeed_down;
            }
        }
    }
    public MovementsActions @Movements => new MovementsActions(this);
    public interface IMovementsActions
    {
        void OnGo_forward(InputAction.CallbackContext context);
        void OnGo_backward(InputAction.CallbackContext context);
        void OnDeflect(InputAction.CallbackContext context);
        void OnTorque_right(InputAction.CallbackContext context);
        void OnTorque_left(InputAction.CallbackContext context);
        void OnActivate(InputAction.CallbackContext context);
        void OnCapture(InputAction.CallbackContext context);
        void OnSpeed_up(InputAction.CallbackContext context);
        void OnSpeed_down(InputAction.CallbackContext context);
    }
}
