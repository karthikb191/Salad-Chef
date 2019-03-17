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

public interface IInteractable
{
    void Interact(Player p);
}

public interface IPickable : IInteractable
{
    void Pick(Player p);
}

public class Vegetable : MonoBehaviour, IPickable {
    bool shredded = false;

    public Veggie veggieType = Veggie.Tomato;
    
    public void Interact(Player p)
    {
        Pick(p);
    }

    public void Pick(Player p)
    {
        //Instantiate a copy and send to the player
        p.AddToItems(this);
    }

}
