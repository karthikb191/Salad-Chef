using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salad : MonoBehaviour, IPickable {

    [HideInInspector]
    public List<Veggie> vegetableList = new List<Veggie>();

    public void AddVegetable(Vegetable v)
    {
        if (!vegetableList.Contains(v.veggieType))
        {
            vegetableList.Add(v.veggieType);
            //Make vegetable a child of the salad
            v.transform.SetParent(transform);
            v.transform.localPosition = new Vector3(1, 1, 0) * Random.Range(-0.7f, 0.7f);
        }
        else
        {
            Debug.Log("Salad already contains the specified type");
        }
    }

    public void ShowPrompt(Player p)
    {

    }

    public void Interact(Player p)
    {
        //disable the collider
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().enabled = false;
        }

        Pick(p);
    }

    public void Pick(Player p)
    {
        p.AddToItems(this);
    }
    

    public bool ContainsVegetable(Vegetable v)
    {
        return vegetableList.Contains(v.veggieType);    
    }
    public bool ContainsVegetable(Veggie v)
    {
        return vegetableList.Contains(v);
    }

    public bool CanServeSalad()
    {
        if (vegetableList.Count > 1)
            return true;

        Debug.Log("Chopping up a single item doesn't make a salad");
        return false;
    }

}
