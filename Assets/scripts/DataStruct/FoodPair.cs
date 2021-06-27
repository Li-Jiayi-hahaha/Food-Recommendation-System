using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPair
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }

    public FoodPair(int num, string str)
    {
        ID = num;
        Name = str;
        Amount = 0;
    }

    public FoodPair(int num, string str, int am)
    {
        ID = num;
        Name = str;
        Amount = am;
    }
}
