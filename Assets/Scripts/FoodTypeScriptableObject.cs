using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "ScriptableObjects/FoodTypeScriptableObject", order = 1)]
public class FoodTypeScriptableObject : ScriptableObject
{
    public string food;
    public int weight, calorieCost;
    public float carbs, protein, fat;
    public int consumingTime;

}