using EditorAttributes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Clamp(0.1f, 10.0f), Tooltip("The speed of the player.")]
    private float _moveSpeed = 2.0f;
       
    public void MoveRight()
    {
        // Debug.Log("MoveRight called on " + gameObject.name);
        transform.position += _moveSpeed * Time.deltaTime * Vector3.right;
    }

    public void MoveLeft()
    {
        // Debug.Log("MoveLeft called on " + gameObject.name);
         transform.position += _moveSpeed * Time.deltaTime * Vector3.left;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
