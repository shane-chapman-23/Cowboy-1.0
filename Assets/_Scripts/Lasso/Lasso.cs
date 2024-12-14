using UnityEngine;

public class Lasso : MonoBehaviour
{
    [SerializeField]
    private LayerMask _whatIsGround;
    [SerializeField]
    private LayerMask _whatIsAnimal;

    [HideInInspector]
    public bool isGrounded;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _whatIsGround) != 0)
        {
            isGrounded = true;
        }

        if (((1 << collision.gameObject.layer) & _whatIsAnimal) != 0)
        {
            LassoController.Instance.SetCurrentLassoedAnimal(collision.gameObject);
        }
    }
}
