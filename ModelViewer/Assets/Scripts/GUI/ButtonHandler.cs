using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonHandler : MonoBehaviour {
    // Config
    [SerializeField] ModelRotationController controller;

    [SerializeField] WaypointButton waypointButtonPrefab;
    [SerializeField] OverwriteDialog overwriteDialogPrefab;
    [SerializeField] DeleteDialogue deleteDialogPrefab;
    [SerializeField] Button addButtonPrefab;
    [SerializeField] Button saveButton;
    [SerializeField] Button deleteButton;

    [SerializeField] int maxWaypoints;
    

    // Params
    bool saveActive = false;
    bool delActive = false;
    int currentWaypoint = 0;

    OverwriteDialog overwriteDialog;
    Button addButton;
    List<WaypointButton> waypoints = new List<WaypointButton>();


    public void SetSaveActive(bool state) {
        saveActive = state;

        if (saveActive) {
            saveButton.GetComponent<Image>().color = Color.yellow;
            // Spawn an add Button
            SpawnAddButton();
        } else {
            saveButton.GetComponent<Image>().color = Color.white;
            // Destroy the current Add Button if it exists
            if (addButton) {
                Destroy(addButton.gameObject);
                addButton=null;
            }
            // Destroy the current Overwrite Dialog if it exists
            if (overwriteDialog) {
                Destroy(overwriteDialog.gameObject);
                overwriteDialog = null;
            }
        }
    }

    public void SetDeleteActive(bool state) {
        delActive = state;
    }

    private void SpawnAddButton() {
        // Check if maxSaves is exceeded
        if (waypoints.Count >= maxWaypoints) {
            return;
        }

        // Spawn the Add Button
        addButton = Instantiate(addButtonPrefab);
        addButton.transform.SetParent(saveButton.transform);
        addButton.transform.localScale = Vector3.one;

        // Position Add Button relative to current waypoint count
        Vector3 posSpacing = new Vector3(0, -74, 0);
        Vector3 finalPos = posSpacing * (waypoints.Count + 1);
        addButton.transform.localPosition = finalPos;

        // Name Button
        addButton.name = $"Add {waypoints.Count + 1}";
    }

    public void OnSaveButtonPressed() {
        // If Save mode is active, cancel it
        if (saveActive) {
            SetSaveActive(false);
            //Destroy(addButton.gameObject);
            return;
        }

        // Activate the Save mode state
        SetSaveActive(true);
    }

    public void OnWaypointButtonPressed(int index) {
        currentWaypoint = index;

        if (saveActive) {
            OverwriteDialog();
            return;
        }

        controller.SetRotation(waypoints[index].Rotation);
    }

    public void OverwriteDialog() {
        if (overwriteDialog) {
            Destroy(overwriteDialog.gameObject);
        }

        WaypointButton parentWaypoint = waypoints[currentWaypoint];

        OverwriteDialog newDialog = Instantiate(overwriteDialogPrefab);
        newDialog.Index = waypoints.Count;
        newDialog.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
        newDialog.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
        newDialog.transform.SetParent(parentWaypoint.transform);
        newDialog.GetComponent<RectTransform>().anchoredPosition = parentWaypoint.GetComponent<RectTransform>().anchoredPosition + new Vector2(-108, 113);
        newDialog.name = $"Overwrite {currentWaypoint}";
        newDialog.transform.localScale = Vector3.one;

        overwriteDialog = newDialog;
    }

    public void HandleOverwrite() {
        // Set selected waypoint rotation to current rotation
        waypoints[currentWaypoint].Rotation = controller.GetRotation();
    }

    public void OnAddButtonPressed() {
        // Replace Add Button by Waypoint
        WaypointButton newWaypoint = Instantiate(waypointButtonPrefab);
        newWaypoint.Index = waypoints.Count;
        newWaypoint.GetComponentInChildren<TMP_Text>().text = (waypoints.Count + 1).ToString();
        newWaypoint.transform.position = addButton.transform.position;
        newWaypoint.transform.SetParent(saveButton.transform);
        newWaypoint.name = $"Waypoint {waypoints.Count + 1}";
        newWaypoint.transform.localScale = Vector3.one;

        // Set Waypoint rotation to current rotation
        newWaypoint.Rotation = controller.GetRotation();

        // Add Waypoint to List
        waypoints.Add(newWaypoint);

        // Deactivate Save Mode
        SetSaveActive(false);
    }

    public void OnDeleteButtonPressed() {
        if (delActive) { return; }

        delActive = true;

        Button parentWaypoint = deleteButton;

        // Spawn the Deletion Dialogue
        DeleteDialogue newDialog = Instantiate(deleteDialogPrefab);

        // Set Anchor Values
        newDialog.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
        newDialog.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
        newDialog.transform.SetParent(parentWaypoint.transform);
        newDialog.GetComponent<RectTransform>().anchoredPosition = parentWaypoint.GetComponent<RectTransform>().anchoredPosition + new Vector2(-81, 55);
        newDialog.name = $"Delete Dialogue";
        newDialog.transform.localScale = Vector3.one;
    }

    public void HandleDeletion() {
        foreach(var waypoint in waypoints) {
            Destroy(waypoint.gameObject);
        }

        waypoints.Clear();
    }
}
