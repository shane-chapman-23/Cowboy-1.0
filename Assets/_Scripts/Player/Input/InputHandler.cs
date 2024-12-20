using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Vector2 _rawMovementInput;
    public int NormInputX {  get; private set; }
    public bool GallopInput { get; private set; }
    public bool LassoInputDown { get; private set; }
    public bool LassoInputUp { get; private set; }

    public bool EscapePressed { get; private set; }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _rawMovementInput = context.ReadValue<Vector2>();

        NormInputX = (int)(_rawMovementInput * Vector2.right).normalized.x;
    }

    public void OnGallopInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GallopInput = true;
        }

        if (context.canceled)
        {
            GallopInput = false;
        }
    }

    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EscapePressed = true;
        }
    }

    public void SetEscapeFalse()
    {
        EscapePressed = false;
    }

    public void OnLassoInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            LassoInputDown = true;
            LassoInputUp = false;
        }

        if (context.canceled)
        {
            LassoInputDown = false;
            LassoInputUp = true;
        }
    }

    public void SetLassoInputUpFalse()
    {
        LassoInputUp = false;
    }
}
