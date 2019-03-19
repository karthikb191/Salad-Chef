using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dustbin : MonoBehaviour, IInteractable {
    

    public void ShowPrompt(Player p)
    {
        p.Text.text = "Press " + p.action + " to throw salad";
    }

    public void Interact(Player p)
    {
        if (p.CarryingSalad())
        {
            p.DestroySalad();
            //Negative points for destroying salad
            p.AddScore(-10);
        }
    }
    
}
