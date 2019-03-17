﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Choppin board can contain one vegetable(being chopped) and a salad
/// </summary>
public class ChoppingBoard : MonoBehaviour, IChop {

    public Player boardAssignedTo;

    Vegetable veggie;
    Salad salad;

    GameObject veggieSlot;
    GameObject saladSlot;

    bool cuttingInProcess = false;

    private void Start()
    {
        veggieSlot = transform.Find("Veggie Slot").gameObject;
        saladSlot = transform.Find("Salad Slot").gameObject;
    }

    //Default interaction with chopping board is to place vegetables on it for chopping
    public void Interact(Player p)
    {
        if (p == boardAssignedTo && !cuttingInProcess && veggie == null)
        {
            //If player is not carrying anything, he can't chop anything
            if (p.GetItemsCarrying().Count == 0)
            {
                Debug.Log("No items to chop. Choose a vegetable first");
                return;
            }

            //Get the first picked vegetable from the list and try chop ot
            for (int i = 0; i < p.GetItemsCarrying().Count; i++)
            {
                if (p.GetItemsCarrying()[i] as Vegetable)
                {
                    //Remove the vegetable from player's list first
                    Vegetable v = p.GetItemsCarrying()[i] as Vegetable;
                    p.RemoveVegetable(v);
                    Chop(v as Vegetable);
                    return;
                }
            }
        }
        else
        {
            if (veggie != null)
                Debug.Log("Veggie is already there...Take the salad out");
            else if (cuttingInProcess)
                Debug.Log("cutting in progress");
            else
                Debug.Log("Use your own board");
        }
    }

    public void Chop(Vegetable v)
    {
        //Make vegetable as a child to the board
        v.transform.SetParent(veggieSlot.transform);
        v.transform.localPosition = Vector3.zero;
        cuttingInProcess = true;
        StartCoroutine(ChopProcess(v));
    }

    IEnumerator ChopProcess(Vegetable v)
    {
        boardAssignedTo.PauseMovement();
        //Wait for a while to finish chopping
        yield return new WaitForSeconds(4);

        v.ChopVegetable();

        //If vegetable slot if empty, then assign it to the vegetable slot of the cutting board
        if(veggie == null)
        {
            if(salad == null)
            {
                MakeSalad(v);
            }
            else
            {
                if (!salad.ContainsVegetable(v))
                    AddToSalad(v);
                else
                    veggie = v;
            }
        }
        else
        {
            Debug.Log("Veggie and salad are not null....Give salad to the customer or take it out");
        }
        
        boardAssignedTo.ResumeMovement();

        cuttingInProcess = false;
    }

    void MakeSalad(Vegetable v)
    {
        //create empty game object
        GameObject g = new GameObject();
        g.transform.SetParent(saladSlot.transform);
        g.transform.localPosition = Vector3.zero;

        salad = g.AddComponent<Salad>();
        salad.AddVegetable(v);

        Debug.Log("Made a salad");
    }


    void AddToSalad(Vegetable v)
    {
        if (salad != null)
        {
            Debug.Log("Added vegetable to salad");
            salad.AddVegetable(v);
        }
        else
        {
            Debug.Log("Something went wrong. Salad is null");
        }
    }

}