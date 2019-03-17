using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPoint : MonoBehaviour, IServable {

    Customer customer;

    public void TakeInCustomer(Customer c)
    {
        customer = c;
        customer.transform.SetParent(transform);
        customer.transform.localPosition = Vector3.zero;
        customer.AssignedToPoint = this;
        customer.DecideOrder();
    }

    public void Interact(Player p)
    {
        Serve(p.GetSalad());
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
    }

}
