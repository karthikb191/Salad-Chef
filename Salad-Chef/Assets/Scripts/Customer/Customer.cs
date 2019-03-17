using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {
    bool angry = false;
    bool satisfied = false;

    float waitPeriod = 15;
    float timeElapsed = 0;

    public List<Veggie> saladCombo = new List<Veggie>();
    
    public CustomerPoint AssignedToPoint { get; set; }

    public void Serve(Salad s)
    {
        if (!IsOrderValid(s))
        {
            angry = true;
        }
    }

    bool IsOrderValid(Salad s)
    {
        for(int i = 0; i < saladCombo.Count; i++)
        {
            if (s.ContainsVegetable(saladCombo[i]))
                continue;
            return false;
        }
        return true;
    }

    
    private void Update()
    {
        //Place Holder code to check instantiation of customers
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= waitPeriod)
        {
            Leave();
        }
    }

    void Leave()
    {
        timeElapsed = 0;
        //Score, etc goes here
        CustomerManager.Instance.RemoveCustomer(this);
    }
}
