using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleFood : MonoBehaviour
{
    public int nurishValue;

    public enum FoodStatus
    {
        Inedible,
        Veggies,
        Meat,
        Alive
    }

    public FoodStatus lifeStatus = FoodStatus.Inedible;
}
