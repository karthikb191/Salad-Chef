using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Veggie
{
    Tomato,
    Potato,
    Cucumber,
    Banana,
    Raddish,
    Carrot
}


public class Vegetable : MonoBehaviour, IPickable {
    bool chopped = false;

    public Veggie veggieType = Veggie.Tomato;
    
    public void ShowPrompt(Player p)
    {
        
        //Get the position of the gameobject in the screen space
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        Debug.Log("Viewport position: " + viewportPosition);

        //Assign the position of the player's instruction panel according to the viewport position of the interactable object
        Vector3 panelPosition = Vector3.zero;
        if(viewportPosition.x < 0.5f)
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
        p.AddToItems(this);
    }

    public void ChopVegetable()
    {
        chopped = true;
    }

}
