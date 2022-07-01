using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _movementSpeed;
    private Rigidbody _rigidbody;
    private Vector3 _movementVector;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        _movementVector.x = Input.GetAxis("Horizontal") * _movementSpeed;
        _movementVector.z = Input.GetAxis("Vertical") * _movementSpeed;

        _rigidbody.velocity = _movementVector;
    }
}
