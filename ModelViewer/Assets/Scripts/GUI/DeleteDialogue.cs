using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDialogue : MonoBehaviour {

    [SerializeField] ButtonHandler buttonHandler;

    public void OnConfirmButtonPressed() {
        Debug.Log("Confirm Pressed");
        buttonHandler.HandleDeletion();
        SetDelInactive();
        Destroy(gameObject);
    }

    public void OnCancelButtonPressed() {
        Debug.Log("Cancel Pressed");
        SetDelInactive();
        Destroy(gameObject);
    }

    public void SetDelInactive() {
        buttonHandler.SetDeleteActive(false);
    }
}
