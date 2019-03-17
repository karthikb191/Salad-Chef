using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {
    bool angry = false;
    bool satisfied = false;
    bool served = false;

    float waitPeriod = 15;
    float timeElapsed = 0;

    public List<Veggie> saladCombo = new List<Veggie>();
    
    public CustomerPoint AssignedToPoint { get; set; }

    public void DecideOrder()
    {
        List<string> saladDishes = new List<string>();
        saladDishes.AddRange(System.Enum.GetNames(typeof(Veggie)));

        int comboCount = Random.Range(2, 4);

        Debug.Log("Order combo count: " + comboCount);

        for(int i = 0; i < comboCount; i++)
        {
            int randomDish = Random.Range(0, saladDishes.Count);
            saladCombo.Add((Veggie)System.Enum.Parse(typeof(Veggie), saladDishes[randomDish]));
            
            saladDishes.RemoveAt(randomDish);
        }

        Debug.Log("order is: ");
        for(int i = 0; i < saladCombo.Count; i++)
        {
            Debug.Log(saladCombo[i]);
        }

        SetWaitPeriod();
    }

    void SetWaitPeriod()
    {
        //Add 15 seconds for every item in the chosen order
        waitPeriod = saladCombo.Count * 15;
    }

    public void ClearOrder()
    {
        saladCombo.Clear();
    }

    public void Serve(Salad s)
    {
        if (!IsOrderValid(s))
        {
            angry = true;
        }
        else
        {
            served = true;
            s.transform.parent = this.transform;
            //Start eating the salad
            StartCoroutine(Eat(s));
        }
    }

    IEnumerator Eat(Salad s)
    {
        for(int i = 0; i < s.transform.childCount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(s.transform.GetChild(0).gameObject);
        }
        Destroy(s);
        Debug.Log("Customer is satisfied");
        Leave();
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
        if (!served)
        {
            //Place Holder code to check instantiation of customers
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= waitPeriod)
            {
                Leave();
            }
        }
    }

    void Leave()
    {
        timeElapsed = 0;
        //Score, etc goes here
        CustomerManager.Instance.RemoveCustomer(this);
    }
}
