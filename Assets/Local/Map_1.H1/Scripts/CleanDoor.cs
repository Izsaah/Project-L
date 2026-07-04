using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ProjectL.Scripts.Interface;

public class CleanDoor : MonoBehaviour
{
    public float openAngle = 90f;
    public float swingSpeed = 1.5f;

    private bool isOpen = false;
    private bool isPlayerNear = false; 
    private Vector3 closedRotation;

    private IInputProvider inputProvider;
    
    void Start(){
        closedRotation = transform.localEulerAngles;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObj != null)
        {
            inputProvider = playerObj.GetComponent<IInputProvider>();
        }
        else
        {
            Debug.LogError("CleanDoor could not find the Player! Make sure your player character has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (isPlayerNear && inputProvider != null && inputProvider.GetInteract())
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        
        // 2. THIS IS THE FIX! We calculate the new angle based on the snapshot we took in Start()
        Vector3 targetRotation = isOpen ? 
            new Vector3(closedRotation.x, closedRotation.y, closedRotation.z + openAngle) : 
            closedRotation;

        // 3. We apply that calculated rotation
        transform.DOLocalRotate(targetRotation, swingSpeed).SetEase(Ease.InOutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = true;
            Debug.Log(gameObject.name + " was triggered by " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;
    }
}