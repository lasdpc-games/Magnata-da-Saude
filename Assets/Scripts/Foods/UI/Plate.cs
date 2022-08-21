using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Foods.UI{
    public class Plate : MonoBehaviour{
        //INSPECTOR VARIABLES
        [SerializeField] private GameObject plateContainer;

        [Header("Food Selection")]
        [SerializeField] private TMP_Text foodName;
        [SerializeField] private Slider weightSlider;
        [SerializeField] private TMP_Text weightValue;
        [SerializeField] private Image[] macroBarChart;

        [Header("Nutritional Info")]
        [SerializeField] private Image[] macroPizzaChart;
        [SerializeField] private TMP_Text totalCalories;

        //PRIVATE VARIABLES
        private FoodTypeScriptableObject selectedFood;


        //BUTTON METHODS
        public void AddCurrentFoodToPlate(){
            if(selectedFood == null)
                return;

            ChangeCurrentFoodQuantity(weightSlider.value);
            GetComponent<Foods.Plate>().AddToPlate(selectedFood);

            Foods.UI.Fridge generalUI = GetComponent<Foods.UI.Fridge>();
            DisplayFoodInPlate(selectedFood, "foodInPlateButton", plateContainer.transform);
            generalUI.SetChartValues(new float[3]{0, 0, 0}, macroBarChart);
            foodName.text = "-";
            //Debug.Log("Cal: " + selectedFood.calorieCost);

            selectedFood = null;
            PlateNutritionalInfo();
        }

        public void UpdateFoodWeight(float weight){
            weightValue.text = weight + selectedFood.measure;
        }

        //PUBLIC METHODS
        public void SelectFood(FoodTypeScriptableObject food){
            selectedFood = food;
            foodName.text = food.food;
            weightSlider.value = food.weight;
            weightValue.text = food.weight + food.measure;

            float[] macros = new float[3]{food.protein, food.fat, food.carbs};    
            GetComponent<Foods.UI.Fridge>().SetChartValues(macros, macroBarChart);
        }

        public void PlateNutritionalInfo(){
            int calories = 0;
            float carbs = 0, proteins = 0, fats = 0;
            foreach(FoodTypeScriptableObject food in GetComponent<Foods.Plate>().FoodPlate){
                calories += food.calorieCost;
                carbs += food.carbs;
                proteins += food.protein;
                fats += food.fat;
            }
            float[] macros = new float[3]{proteins, fats, carbs};
            PizzaGraph(macros, calories);
        }

        public void RemoveFood(){
            List<FoodTypeScriptableObject> plate = GetComponent<Foods.Plate>().FoodPlate;

            foreach(FoodTypeScriptableObject item in plate){
                Destroy(item.go);
            }
            plate.Clear();

            float[] zero = new float[3]{0, 0, 0};
            PizzaGraph(zero, 0);
        }

        //PRIVATE METHODS
        void DisplayFoodInPlate(FoodTypeScriptableObject food, string template, Transform location){
            GameObject newFoodType = Instantiate(Resources.Load(template, typeof(GameObject)), location) as GameObject;

            newFoodType.GetComponent<Foods.UI.FoodSelectionButton>().FoodValues = food;

            TMP_Text name = newFoodType.transform.Find("Food").GetComponent<TMP_Text>();
            TMP_Text weight = newFoodType.transform.Find("Weight").GetComponent<TMP_Text>();
            
            name.text = food.food;
            weight.text = food.weight + food.measure;
            food.go = newFoodType;
        }

        void ChangeCurrentFoodQuantity(float weight){
            selectedFood.calorieCost = ((int)weight * selectedFood.calorieCost)/100;
            selectedFood.carbs = (weight * selectedFood.carbs)/100;
            selectedFood.fat = (weight * selectedFood.fat)/100;
            selectedFood.protein = (weight * selectedFood.protein)/100;
            selectedFood.weight = (int)weight;
        }

        void PizzaGraph(float[] macros, int calories){
            Foods.UI.Fridge generalUI = GetComponent<Foods.UI.Fridge>();
            generalUI.SetChartValues(macros, macroPizzaChart);
            totalCalories.text = calories + "kcal";
        }
    }
}