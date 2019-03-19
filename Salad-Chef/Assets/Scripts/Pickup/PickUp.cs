using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    AddTime,
    AddScore,
    AddSpeed
}

public class PickUp : MonoBehaviour, IPickable
{
    Player givenTo;

    public PickUpType pickUpType;

    public void ShowPrompt(Player p)
    {

        //Get the position of the gameobject in the screen space
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        Debug.Log("Viewport position: " + viewportPosition);

        //Assign the position of the player's instruction panel according to the viewport position of the interactable object
        Vector3 panelPosition = Vector3.zero;
        if (viewportPosition.x < 0.5f)
        {
            panelPosition.x = Player.panelXOffset;
        }
        else
        {
            panelPosition.x = -Player.panelXOffset;
        }

        p.InstructionPanel.gameObject.SetActive(true);
        p.InstructionPanel.localPosition = panelPosition;
        p.Text.text = "Press " + p.action + " to pick up";
    }

    public void Interact(Player p)
    {
        Pick(p);
    }

    public void Pick(Player p)
    {
        switch (pickUpType)
        {
            case PickUpType.AddScore:
                p.AddScore(50);
                break;
            case PickUpType.AddSpeed:
                p.AddSpeed(2);
                break;
            case PickUpType.AddTime:
                p.AddTime(30);
                break;
        }
        Destroy(this.gameObject);
    }

    public void Spawn(Player givenTo, Vector3 positioToSpawn)
    {
        Debug.Log("Spawned the pickup");

        PickUp p = Instantiate(this);
        p.transform.position = positioToSpawn;
        this.givenTo = givenTo;
    }

}
