using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void Update()
    {
        Vector2 InputVector = new Vector2(0, 0);

        if (Keyboard.current.wKey.isPressed)
        {
            InputVector.y = 1f;
        }
    }
}
