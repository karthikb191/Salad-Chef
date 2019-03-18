using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour {

    public Player playerAssigned;

    public Text ScoreText { get; set; }
    public Text TimeText { get; set; }
    public Text ItemsText { get; set; }


    private void Start()
    {
        ScoreText = transform.Find("Score Panel").Find("Value").GetComponent<Text>();
        TimeText = transform.Find("Time Panel").Find("Value").GetComponent<Text>();
        ItemsText = transform.Find("Items Panel").Find("Value").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        UpdateUI();
	}

    void UpdateUI()
    {
        if (!playerAssigned) return;

        ScoreText.text = playerAssigned.Score.ToString();
        TimeText.text = playerAssigned.TimeLeft.ToString();

        ItemsText.text = "";
        List<Object> itemsCarrying = playerAssigned.GetItemsCarrying();

        for (int i = 0; i < itemsCarrying.Count; i++)
        {
            
            ItemsText.text += "<size=17>" + (i+1) + "</size>" + "\n";
            
            ItemsText.text += GetItem(itemsCarrying[i]);
            
        }
    }

    string GetItem(Object o)
    {
        string res = "";
        if(o as Vegetable)
        {
            Vegetable v = o as Vegetable;
            res = v.veggieType.ToString() + "\n";
            return res;
        }
        else
        {
            if(o as Salad)
            {
                Salad s = o as Salad;
                res += "<size=17>" + "<b>" + "SALAD" + "</b>" + "</size>" + "\n";
                for(int i = 0; i < s.vegetableList.Count; i++)
                {
                    if (i != s.vegetableList.Count - 1)
                    {
                        res += s.vegetableList[i].ToString() + "\n";
                        res += "+" + "\n";
                        
                    }
                    else
                    {
                        res += s.vegetableList[i].ToString() + "\n";
                    }
                }
            }
        }
        return res;
    }

}
