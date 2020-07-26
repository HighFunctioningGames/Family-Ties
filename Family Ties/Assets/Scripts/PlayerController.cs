using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    ///----------------------------
    ///    Player Variables
    ///----------------------------
    [Tooltip("Value used to determine the lateral movement speed on the character")]
    [SerializeField]float speed;
    [Tooltip("Value used to determine the magnitude of the force applied in a jump(This is set at object start and cannot be modified)")]
    [SerializeField]float jumpForce;
    [Tooltip("Value used to determine the magnitude of the force applied when bouncing off another player(This is set at object start and cannot be modifed)")]
    [SerializeField]float bounceForce;


    Vector3 jumpVector;
    Vector3 bounceVector;

    bool isGrounded;
    bool atStairs;


    ///---------------------------
    ///  Player Control Strings
    ///---------------------------
    [Tooltip("String related to a keyboard input for movement")]
    [SerializeField]string left;
    [Tooltip("String related to a keyboard input for movement")]
    [SerializeField]string right;
    [Tooltip("String related to a keyboard input for movement")]
    [SerializeField]string jump;
    [Tooltip("String related to a keyboard input for moving up stairs")]
    [SerializeField]string up;
    [Tooltip("String related to a keyboard input for moving down stairs")]
    [SerializeField]string down;

    Rigidbody rbody;
    Stairs storedStairs = null;
    GameObject stairsTrigger = null;

    void Start()
    {
        rbody           = GetComponent<Rigidbody>();
        jumpVector      = new Vector3(0, jumpForce, 0);
        bounceVector    = new Vector3(0, bounceForce, 0);
        isGrounded      = false;
        atStairs        = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!isGrounded)
            {
                float dif = transform.position.y - collision.gameObject.transform.position.y;
                if (dif >= 0.5f)
                {
                    rbody.AddForce(bounceVector, ForceMode.Impulse);
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            atStairs = false;
            storedStairs = null;
            Debug.Log("Exited Stairs");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stairs"))
        {
            atStairs = true;
            stairsTrigger = other.gameObject;
            storedStairs = other.gameObject.GetComponentInParent<Stairs>();
            Debug.Log("Entered Stairs");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Stairs"))
        {
            atStairs = false;
            stairsTrigger = null;
            storedStairs = null;
            Debug.Log("Exited Stairs");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(up) && atStairs)
        {
            if(storedStairs != null)
            {
                if(storedStairs.CheckUp(stairsTrigger))
                {
                    transform.position = storedStairs.MoveUp().position;
                }
            }
        }
        else if(Input.GetKeyDown(down) && atStairs)
        {
            if (storedStairs != null)
            {
                if(storedStairs.CheckDown(stairsTrigger))
                {
                    transform.position = storedStairs.MoveDown().position;
                }
            }
        }
    }


    void FixedUpdate()
    {
        if(Input.GetKey(left))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if(Input.GetKey(right))
        {
           transform.Translate(speed * Time.deltaTime, 0, 0);
        }

        if(Input.GetKeyDown(jump) && isGrounded)
        {
            rbody.AddForce(jumpVector, ForceMode.Impulse);
            isGrounded = false;
        }
    }
}
