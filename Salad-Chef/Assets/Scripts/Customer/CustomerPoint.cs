using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerPoint : MonoBehaviour, IServable {

    Customer customer;

    public GameObject CustomerPanel { get; set; }
    public Text Text { get; set; }
    public Slider waitSlider { get; set; }

    private void Start()
    {
        CustomerPanel = transform.Find("Customer Canvas").GetChild(0).gameObject;
        Text = CustomerPanel.transform.GetChild(0).GetComponent<Text>();
        waitSlider = transform.Find("Customer Canvas").Find("Wait Slider").GetComponent<Slider>();
        Text.text = "";
        //Deactivate the panels initially when the game starts
        CustomerPanel.SetActive(false);
        waitSlider.gameObject.SetActive(false);
    }

    public void TakeInCustomer(Customer c)
    {
        customer = c;
        customer.transform.SetParent(transform);
        customer.transform.localPosition = Vector3.zero;
        customer.AssignedToPoint = this;

        //Let the customer decide the order
        CustomerPanel.SetActive(true);
        customer.DecideOrder();

        //enable the wait slider also
        waitSlider.gameObject.SetActive(true);
    }

    public void ShowPrompt(Player p)
    {
        if (HasCustomer())
        {
            //Get the position of the gameobject in the screen space
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            Debug.Log("Viewport position: " + viewportPosition);

            //Assign the position of the player's instruction panel according to the viewport position of the interactable object
            Vector3 panelPosition = Vector3.zero;

            if (viewportPosition.y > 0.5)
            {
                panelPosition.y = -Player.panelYOffset;
            }
        
            p.InstructionPanel.gameObject.SetActive(true);
            p.InstructionPanel.localPosition = panelPosition;
            p.Text.fontSize = 75;
            p.Text.text = "Press " + p.action + " to serve salad \n";
        }
    }
    
    public void Interact(Player p)
    {
        Serve(p.ServeSalad());
        //Remove salad from the player
        p.RemoveSalad();
    }

    public void Serve(Salad s)
    {
        //Only serve if there;s a customer
        if (HasCustomer())
        {
            if(s == null)
            {
                Debug.Log("There's no salad to serve");
            }
            else
            {
                customer.Serve(s);
            }
        }
    }

    public bool HasCustomer()
    {
        if (customer) return true;
        return false;
    }

    public void RemoveCustomer()
    {
        customer.transform.parent = null;
        customer.ClearOrder();
        customer = null;

        //Reset the UI
        Text.text = "";
        CustomerPanel.SetActive(false);

        //Disable the wait slider also
        waitSlider.gameObject.SetActive(false);
    }

}
