using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(Player p);
}

public interface IPickable : IInteractable
{
    void Pick(Player p);
}

public interface IChop : IInteractable
{
    void Chop(Vegetable v);
}
