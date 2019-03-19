using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour, IInteractable {

    Vegetable vegetableOnPlate;
    public Player assignedTo;

    public void ShowPrompt(Player p)
    {
        if(p == assignedTo)
        {
            p.Text.text = "Press " + p.action + " to put vegggie on plate";
        }
    }

    public void Interact(Player p)
    {
        if(p == assignedTo)
        {
            if(vegetableOnPlate == null)
            {
                for(int i = 0; i < p.GetItemsCarrying().Count; i++)
                {
                    Vegetable v = p.GetItemsCarrying()[i] as Vegetable;
                    if (v)
                    {
                        p.RemoveVegetable(v);
                        v.transform.parent = this.transform;
                        v.transform.localPosition = Vector3.zero;
                        v.transform.localScale = Vector3.one;
                        vegetableOnPlate = v;
                    }
                }
            }
            else
            {
                if (p.TransferItem(vegetableOnPlate))
                {
                    Debug.Log("Transfer successful");
                    vegetableOnPlate = null;
                }
            }
        }
    }
    
}
