using System.Collections;
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

    public void ShowPrompt(Player p)
    {
        //Get the position of the gameobject in the screen space
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        Debug.Log("Viewport position: " + viewportPosition);

        //Assign the position of the player's instruction panel according to the viewport position of the interactable object
        Vector3 panelPosition = Vector3.zero;

        if(viewportPosition.y < 0.5)
        {
            panelPosition.y = Player.panelYOffset;
        }

        p.InstructionPanel.sizeDelta = new Vector2(p.InstructionPanel.sizeDelta.x, Player.panelHeight * 2);
        p.Text.GetComponent<RectTransform>().sizeDelta = p.InstructionPanel.sizeDelta;
        p.InstructionPanel.gameObject.SetActive(true);
        p.InstructionPanel.localPosition = panelPosition;
        p.Text.fontSize = 70;
        p.Text.text = "Press " + p.action + " to chop vegetable \n" + 
                      "Press " + p.pickUpSalad + " to pick up salad";
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
        yield return new WaitForSeconds(1);

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

    public void AddSaladToPlayer(Player p)
    {
        if (salad.CanServeSalad())
        {
            salad.Interact(p);
            //salad object must be set to null as the salad is not on the chopping board, but with player instead
            salad = null;
        }
        if(salad == null)
        {
            if(veggie != null)
            {
                MakeSalad(veggie);
            }
        }
    }

}
