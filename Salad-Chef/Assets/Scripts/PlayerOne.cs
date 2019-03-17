using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode action;
    public KeyCode pickUpSalad;

    public float speed = 10;    //Default speed of the player
    public int maxCarryCapacity = 2;

    float multiplier = 1;
    bool pauseMovement = false;

    GameObject rightHand;
    GameObject leftHand;

    Vector3 Move()
    {
        Vector3 displacement = Vector3.zero;
        bool keyPressed = false;
        

        if (Input.GetKey(up)) { displacement.y += multiplier * speed * Time.deltaTime; keyPressed = true; }
        if (Input.GetKey(down)) { displacement.y -= multiplier * speed * Time.deltaTime; keyPressed = true; }
        if (Input.GetKey(left)) { displacement.x -= multiplier * speed * Time.deltaTime; keyPressed = true; }
        if (Input.GetKey(right)) { displacement.x += multiplier * speed * Time.deltaTime; keyPressed = true; }

        //TODO: ADD the smooth movement logic and direction interpolation
        //TODO: Fix unity collider issue
        if (keyPressed)
        {

        }
        else
        {

        }

        return displacement;
    }

    List<Object> itemsCarrying = new List<Object>();

    public delegate void Action();
    public event Action ActionEvent;

    IInteractable interactableObjectOnFocus;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IInteractable i = collider.GetComponent<IInteractable>();
        if(i != null)
        {
            //Debug.Log("Came into contact with interactable object");
            interactableObjectOnFocus = i;
        }
        else
        {
            Debug.Log("meh");
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        IInteractable i = collider.GetComponent<IInteractable>();
        if (interactableObjectOnFocus == i)
        {
            interactableObjectOnFocus = null;
        }
    }

    protected void Start()
    {
        rightHand = transform.Find("Right Hand").gameObject;
        leftHand = transform.Find("Left Hand").gameObject;
        //itemsCarrying = new ArrayList(2);
    }

    protected void Update()
    {
        if(!pauseMovement)
            gameObject.transform.Translate(Move());

        if (Input.GetKeyDown(action))
        {
            Interact(interactableObjectOnFocus);
            //PickUp(pickableObjectOnFocus);
        }
        if (Input.GetKeyDown(pickUpSalad))
        {
            PickUpSalad(interactableObjectOnFocus);
        }
    }

    void PickUpSalad(IInteractable obj)
    {
        ChoppingBoard cb = obj as ChoppingBoard;
        if (cb)
        {
            Debug.Log("chopping board access is a success");
            cb.AddSaladToPlayer(this);
        }
        else
        {
            Debug.Log("Not chopping Board");
        }
    }

    public List<Object> GetItemsCarrying()
    {
        return itemsCarrying;
    }

    public void Interact(IInteractable obj)
    {
        if(obj != null)
        {
            Debug.Log("Executing the interaction");
            obj.Interact(this);
        }
        else
        {
            Debug.Log("Interactable object is null");
        }
    }

    public void AddToItems(Object obj)
    {
        if(itemsCarrying.Count >= maxCarryCapacity)
        {
            Debug.Log("Carry capacity at maximum");
            return;
        }

        //Check to see if the vegetable we are trying to pick up is already with the player
        Vegetable v = obj as Vegetable;
        if (v)
        {
            Debug.Log("Trying to pickup a vegetable");
            Debug.Log("vegetable is: " + v.veggieType);
            for(int i = 0; i < itemsCarrying.Count; i++)
            {
                Vegetable v2 = itemsCarrying[i] as Vegetable;
                if (v2)
                {
                    if(v.veggieType == v2.veggieType)
                    {
                        Debug.Log("Putting vegetable back");
                        RemoveVegetable(v2, true);
                        return;
                    }
                }
            }
            //If the vegetable type is not being carried, clone it 
            Vegetable clone = Instantiate(v);

            //Assign to one of the hands
            if (rightHand.transform.childCount == 0)
            {
                clone.transform.SetParent(rightHand.transform);
            }
            else
            {
                clone.transform.SetParent(leftHand.transform);
            }
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localScale /= 2;

            //add it to the player's list
            itemsCarrying.Add(clone);
            return;
        }
        
        Salad s = obj as Salad;
        if (s)
        {
            //No special conditions required for salad as it's only possible to make one salad at a time
            Debug.Log("Trying to pickup the salad");

            //Salad can't be cloned
            //Assign to one of the hands
            if (rightHand.transform.childCount == 0)
            {
                s.transform.SetParent(rightHand.transform);
                s.transform.localPosition = Vector3.zero;
            }
            else
            {
                s.transform.SetParent(leftHand.transform);
                s.transform.localPosition = Vector3.zero;
            }

            itemsCarrying.Add(s);
        }
        
    }

    public void PowerUp()
    {

    }
    public Salad GetSalad()
    {
        for (int i = 0; i < itemsCarrying.Count; i++)
        {
            if (itemsCarrying[i] as Salad)
                return itemsCarrying[i] as Salad;
        }
        return null;
    }

    public void RemoveSalad()
    {
        for(int i = 0; i < itemsCarrying.Count; i++)
        {

        }
        Debug.Log("No salad to remove");
    }

    public void RemoveVegetable(Vegetable v, bool destroy = false)
    {
        //Remove from the player's hand
        v.transform.parent = null;
        //Remove from the list
        itemsCarrying.Remove(v);

        if (destroy)
        {
            Destroy(v.gameObject);
        }
    }

    public void PauseMovement()
    {
        pauseMovement = true;
    }
    public void ResumeMovement()
    {
        pauseMovement = false;
    }

}

public class PlayerOne : Player {

    //Constructor defines the default controls of the player
    public PlayerOne()
    {
        up = KeyCode.W;
        down = KeyCode.S;
        left = KeyCode.A;
        right = KeyCode.D;
        action = KeyCode.X;
        pickUpSalad = KeyCode.C;
    }

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
