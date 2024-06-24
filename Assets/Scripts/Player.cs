using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float interactionRadius;

    private float inputH;
    private float inputV;
    private bool moving;
    private Vector3 destinationPoint;
    private Vector3 interactionPoint;
    private Vector3 lastInput;
    private Collider2D forwardCollider;
    private Animator anim;
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int InputH = Animator.StringToHash("inputH");
    private static readonly int InputV = Animator.StringToHash("inputV");

    private bool interacting;

    public bool Interacting
    {
        get => interacting;
        set => interacting = value;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ReadInputs();

        MovementAndAnimations();
    }

    private void MovementAndAnimations()
    {
        if (!interacting && !moving && (inputH != 0 || inputV != 0))
        {
            anim.SetBool(Moving, true);
            anim.SetFloat(InputH, inputH);
            anim.SetFloat(InputV, inputV);
            lastInput = new Vector3(inputH, inputV, 0);
            destinationPoint = transform.position + lastInput;
            interactionPoint = destinationPoint;
            forwardCollider = DoCheck();

            if (!forwardCollider)
            {
                StartCoroutine(Move());
            }
        } else if (inputH == 0 && inputV == 0)
        {
            anim.SetBool(Moving, false);
        }
    }

    private void ReadInputs()
    {
        if (inputV == 0)
        {
            inputH = Input.GetAxisRaw("Horizontal");
        }

        if (inputH == 0)
        {
            inputV = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RunInteraction();
        }
    }

    private void RunInteraction()
    {
        forwardCollider = DoCheck();
        if (forwardCollider)
        {
            if (forwardCollider.TryGetComponent(out Interactive interactive))
            {
                interactive.Interact();
            }
        }
    }

    private IEnumerator Move()
    {
        moving = true;
        while (transform.position != destinationPoint)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, destinationPoint, movementSpeed * Time.deltaTime);
            yield return null;
        }

        interactionPoint = transform.position + lastInput;
        moving = false;
    }

    private Collider2D DoCheck()
    {
        return Physics2D.OverlapCircle(interactionPoint, interactionRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(interactionPoint, interactionRadius);
    }
}