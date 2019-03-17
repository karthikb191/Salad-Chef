using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour {

    public static CustomerManager Instance { get; set; }


    public List<Sprite> customerSprites;
    public GameObject customerPrefab;

    List<CustomerPoint> customerPoints = new List<CustomerPoint>();

    List<Customer> activeCustomers = new List<Customer>();
    List<Customer> deactiveCustomers = new List<Customer>();

    List<int> availableCustomerPointIndices = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        customerPoints.AddRange(FindObjectsOfType<CustomerPoint>());

        for(int i = 0; i < customerPoints.Count; i++)
        {
            availableCustomerPointIndices.Add(i);
        }
	}

    float counter = 0;
    float waitPeriod = 2;

	// Update is called once per frame
	void Update () {

        CustomerInstantiation();
	}

    void CustomerInstantiation()
    {
        counter += Time.deltaTime;

        if (counter >= waitPeriod)
        {
            counter = 0;
            
            //If there are slots available for customer instantiation
            if(availableCustomerPointIndices.Count > 0)
            {
                int slot = Random.Range(0, availableCustomerPointIndices.Count);
                int index = availableCustomerPointIndices[slot];
                int randomIndexToCheck = Random.Range(0, customerPoints.Count);
                if (!customerPoints[randomIndexToCheck].HasCustomer())
                {
                    Customer c;
                    if (deactiveCustomers.Count > 0)
                    {
                        c = deactiveCustomers[0];
                        c.gameObject.SetActive(true);
                        deactiveCustomers.RemoveAt(0);
                        activeCustomers.Add(c);
                    }
                    else
                    {
                        c = Instantiate(customerPrefab).GetComponent<Customer>();
                        activeCustomers.Add(c);
                    }

                    //Remove from the available slot to indicate that the slot is currently full
                    availableCustomerPointIndices.RemoveAt(slot);
                    customerPoints[index].TakeInCustomer(c);
                }
            }
        }
    }

    public void RemoveCustomer(Customer c)
    {
        //Add the index of the customer point to indicate that the place is accepting new customer
        availableCustomerPointIndices.Add(customerPoints.IndexOf(c.AssignedToPoint));
        Debug.Log("Index of: " + customerPoints.IndexOf(c.AssignedToPoint));
        Debug.Log("Available points: " + availableCustomerPointIndices.Count);


        //Remove from the assigned point
        c.AssignedToPoint.RemoveCustomer();
        c.AssignedToPoint = null;
        c.gameObject.SetActive(false);

        activeCustomers.Remove(c);
        deactiveCustomers.Add(c);

    }

}
