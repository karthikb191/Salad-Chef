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
