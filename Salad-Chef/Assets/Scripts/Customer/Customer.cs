using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {
    bool angry = false;
    bool satisfied = false;
    bool served = false;

    float waitPeriod = 15;
    float timeLeft;

    Player servedBy;
    List<Player> wrongServeBy = new List<Player>();

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

        //Once the customer has decided the order, enable the canvas and show the order
        Debug.Log("order is: ");
        for(int i = 0; i < saladCombo.Count; i++)
        {
            if(i != saladCombo.Count - 1)
            {
                AssignedToPoint.Text.text += saladCombo[i] + "\n";
            }
            else
            {
                AssignedToPoint.Text.text += saladCombo[i];
            }

            Debug.Log(saladCombo[i]);
        }

        SetWaitPeriod();
    }

    void SetWaitPeriod()
    {
        //Add 15 seconds for every item in the chosen order
        waitPeriod = saladCombo.Count * CustomerManager.Instance.defaultWaitPeriod;
        timeLeft = waitPeriod;

        //Set the max value of the wait slider
        AssignedToPoint.waitSlider.minValue = 0;
        AssignedToPoint.waitSlider.maxValue = timeLeft;
        AssignedToPoint.waitSlider.value = timeLeft;
    }

    public void ClearOrder()
    {
        saladCombo.Clear();
    }

    public void Serve(Salad s, Player p)
    {
        if (!IsOrderValid(s))
        {
            angry = true;
            //TODO change the character color or something

            //Add to the customer's order list that he is angry
            AssignedToPoint.Text.text += "\n" + "<b><i><color=#ff0000>" + "Customer Angry destroyed salad" + "</color></i></b>";
            if (!wrongServeBy.Contains(p)) { wrongServeBy.Add(p); }
            //Destroy salad
            Destroy(s.gameObject);
        }
        else
        {
            served = true;
            servedBy = p;
            s.transform.parent = this.transform;
            //Start eating the salad
            StartCoroutine(Eat(s));

            //If served before 70% of time, must spawn a pickup. Also, the customer must not have been angry previously
            TrySpawnPickUp(p);
        }
    }

    void TrySpawnPickUp(Player p)
    {
        if(timeLeft / waitPeriod > 0.3f && !angry)
        {
            Vector3 positionToSpawn = Vector3.zero;
            positionToSpawn.x = Random.Range(CustomerManager.Instance.rectangleToSpawnPickups.bounds.min.x,
                                            CustomerManager.Instance.rectangleToSpawnPickups.bounds.max.x);
            positionToSpawn.y = Random.Range(CustomerManager.Instance.rectangleToSpawnPickups.bounds.min.y,
                                            CustomerManager.Instance.rectangleToSpawnPickups.bounds.max.y);

            int randNum = Random.Range(0, CustomerManager.Instance.possiblePickups.Count);
            
            CustomerManager.Instance.possiblePickups[randNum].Spawn(p, positionToSpawn);
            
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
        servedBy.AddScore(100);
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
            timeLeft -= Time.deltaTime;
            AssignedToPoint.waitSlider.value = timeLeft;
            if(timeLeft <= 0)
            {
                Leave();
            }
        }
    }

    void Leave()
    {
        if (!served)
        {
            if (angry)
            {
                for(int i = 0; i < wrongServeBy.Count; i++)
                {
                    //Minus double points for whoever served the wrong order
                    wrongServeBy[i].AddScore(-CustomerManager.Instance.DefaultScore * 2);
                }
            }
            else
            {
                CustomerManager.Instance.CustomerLeftHungry();
            }
        }

        servedBy = null;
        //Score, etc goes here
        CustomerManager.Instance.RemoveCustomer(this);
    }
}
