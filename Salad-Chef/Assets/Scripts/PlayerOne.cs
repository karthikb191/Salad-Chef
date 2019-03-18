using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode action;
    public KeyCode pickUpSalad;


    public float TimeLeft { set; get; }
    public float Score { set; get; }
    public float Speed { set; get; }
    public float defaultTime = 300;   //Setting default time to 300 seconds(5 minutes)
    public float defaultSpeed = 6;    //Default speed of the player
    public int maxCarryCapacity = 2;

    float multiplier = 1;
    bool pauseMovement = false;

    GameObject rightHand;
    GameObject leftHand;

    public static float panelXOffset = 550;
    public static float panelYOffset = 160;
    public static float panelHeight = 110;
    public RectTransform InstructionPanel { get; set; }
    public Text Text { get; set; }
    
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
            interactableObjectOnFocus.ShowPrompt(this);
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

            //Deactivate the panel when the interactable object is null
            InstructionPanel.gameObject.SetActive(false);
            Text.text = "";
            Text.fontSize = 90;
            InstructionPanel.sizeDelta = new Vector2(InstructionPanel.sizeDelta.x, panelHeight);
            Text.GetComponent<RectTransform>().sizeDelta = InstructionPanel.sizeDelta;
        }
    }

    protected void Start()
    {
        TimeLeft = defaultTime; Score = 0; Speed = defaultSpeed;


        rightHand = transform.Find("Right Hand").gameObject;
        leftHand = transform.Find("Left Hand").gameObject;
        //itemsCarrying = new ArrayList(2);

        //Get the canvas components
        InstructionPanel = transform.Find("Player Canvas").transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Text = InstructionPanel.transform.GetChild(0).GetComponent<Text>();

        //deactivate the instruction panel initially
        InstructionPanel.gameObject.SetActive(false);
    }

    protected void Update()
    {
        if (!GameManager.Instance.Paused)
        {
            TimeLeft -= Time.deltaTime;

            if(!pauseMovement)
                gameObject.transform.Translate(Move());

            if (Input.GetKeyDown(action))
            {
                Interact(interactableObjectOnFocus);
            }
            if (Input.GetKeyDown(pickUpSalad))
            {
                PickUpSalad(interactableObjectOnFocus);
            }
        }
    }

    Vector3 Move()
    {
        Vector3 displacement = Vector3.zero;
        bool keyPressed = false;


        if (Input.GetKey(up)) { displacement.y += multiplier * Speed * Time.deltaTime; keyPressed = true; }
        if (Input.GetKey(down)) { displacement.y -= multiplier * Speed * Time.deltaTime; keyPressed = true; }
        if (Input.GetKey(left)) { displacement.x -= multiplier * Speed * Time.deltaTime; keyPressed = true; }
        if (Input.GetKey(right)) { displacement.x += multiplier * Speed * Time.deltaTime; keyPressed = true; }

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

    public void AddSpeed(float amount)
    {
        Speed = Speed >= 25 ? Speed : Speed + amount;
    }
    public void AddScore(float amount)
    {
        Score += amount;
    }
    public void AddTime(float amount)
    {
        TimeLeft += amount;
    }

    /// <summary>
    /// returns salad being carried by the player. Also removes from the player's currently carrying items list
    /// </summary>
    /// <returns> first salad item carried by item. null, if not being carried</returns>
    public Salad ServeSalad()
    {
        for (int i = 0; i < itemsCarrying.Count; i++)
        {
            if (itemsCarrying[i] as Salad)
            {
                Salad s = itemsCarrying[i] as Salad;
                itemsCarrying.RemoveAt(i);
                return s;
            }
        }
        return null;
    }

    public void RemoveSalad()
    {
        for (int i = 0; i < itemsCarrying.Count; i++)
        {
            if (itemsCarrying[i] as Salad)
            {
                Salad s = itemsCarrying[i] as Salad;
                itemsCarrying.RemoveAt(i);
                return;
            }
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

    //public bool CarryingVeggie(Veggie veggieType)
    //{
    //    for(int i = 0; i < itemsCarrying.Count; i++)
    //    {
    //
    //    }
    //}

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
