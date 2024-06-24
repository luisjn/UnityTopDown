using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour, Interactive
{
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField, TextArea(1, 5)] private string[] phrases;
    [SerializeField] private float timeBetweenLetters;
    [SerializeField] private GameObject dialogueFrame;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float waitingTime;
    [SerializeField] private float maxDistance;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask isObstacle;

    private bool talking = false;
    private int currentIndex = -1;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    
    private void Awake()
    {
        initialPosition = transform.position;
    }

    // private void Start()
    // {
    //     StartCoroutine(GoToPositionAndWait());
    // }

    private IEnumerator GoToPositionAndWait()
    {
        while (true)
        {
            CalculateNewTargetPosition();

            while (transform.position != targetPosition)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitingTime);
        }
    }

    private void CalculateNewTargetPosition()
    {
        bool validTile = false;

        while (!validTile)
        {
            int prob = Random.Range(0, 4);
            if (prob == 0)
            {
                targetPosition = transform.position + Vector3.left;
            }
            else if (prob == 1)
            {
                targetPosition = transform.position + Vector3.right;
            }
            else if (prob == 2)
            {
                targetPosition = transform.position + Vector3.up;
            }
            else
            {
                targetPosition = transform.position + Vector3.down;
            }

            validTile = FreeTileAndInside();
        }
    }

    private bool FreeTileAndInside()
    {
        if (Vector3.Distance(initialPosition, targetPosition) > maxDistance)
        {
            return false;
        }
        else
        {
            return !Physics2D.OverlapCircle(targetPosition, detectionRadius, isObstacle);
        }
    }

    public void Interact()
    {
        gameManager.SetPlayerState(false);
        dialogueFrame.SetActive(true);
        if (!talking)
        {
            NextPhrase();
        }
        else
        {
            CompletePhrase();
        }
    }

    private void NextPhrase()
    {
        currentIndex++;
        if (currentIndex >= phrases.Length)
        {
            FinishDialogue();
        }
        else
        {
            StartCoroutine(WritePhrase());
        }
    }

    private void FinishDialogue()
    {
        talking = false;
        dialogueText.text = "";
        currentIndex = -1;
        dialogueFrame.SetActive(false);
        gameManager.SetPlayerState(true);
    }

    private IEnumerator WritePhrase()
    {
        talking = true;
        dialogueText.text = "";
        char[] phraseCharacters = phrases[currentIndex].ToCharArray();
        foreach (var character in phraseCharacters)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        talking = false;
    }

    private void CompletePhrase()
    {
        StopAllCoroutines();
        dialogueText.text = phrases[currentIndex];
        talking = false;
    }
}