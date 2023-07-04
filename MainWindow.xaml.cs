using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace recipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            int numberOfIngredients = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter the number of ingredients:", "Number of Ingredients"));

            for (int i = 0; i < numberOfIngredients; i++)
            {
                string ingredientName = Microsoft.VisualBasic.Interaction.InputBox($"Enter the name of ingredient #{i + 1}:", "Ingredient Name");
                string ingredientQuantity = Microsoft.VisualBasic.Interaction.InputBox($"Enter the quantity of {ingredientName}:", "Ingredient Quantity");
                string ingredientUnit = Microsoft.VisualBasic.Interaction.InputBox($"Enter the unit of measurement for {ingredientName}:", "Ingredient Unit");

                string ingredient = $"{ingredientQuantity} {ingredientUnit} of {ingredientName}";
                ingredientsListBox.Items.Add(ingredient);
            }

        }


        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            string step = Microsoft.VisualBasic.Interaction.InputBox("Enter a step description:", "Add Step");
            if (!string.IsNullOrEmpty(step))
            {
                stepsListBox.Items.Add(step);
            }
        }

       

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipe = new Recipe();
            recipe.Name = recipeNameTextBox.Text;

            while (true)
            {
                Ingredient ingredient = new Ingredient();

                string ingredientName = Microsoft.VisualBasic.Interaction.InputBox("Enter ingredient name (or 'quit' to finish):", "Ingredient Name");
                if (ingredientName.ToLower() == "quit")
                    break;

                ingredient.Name = ingredientName;
                ingredient.Calories = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter ingredient calories:", "Ingredient Calories"));
                ingredient.FoodGroup = Microsoft.VisualBasic.Interaction.InputBox("Enter ingredient food group:", "Ingredient Food Group");

                recipe.Ingredients.Add(ingredient);
            }

            // Add the steps to the recipe
            foreach (var item in stepsListBox.Items)
            {
                recipe.Steps.Add(item.ToString());
            }


            recipes.Add(recipe);
            MessageBox.Show("Recipe successfully added!", "Success");

            ClearInputFields();
        }

        private void ViewRecipeList_Click(object sender, RoutedEventArgs e)
        {
            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes found.", "Recipe List");
                return;
            }

            var sortedRecipes = recipes.OrderBy(r => r.Name);
            string recipeNames = string.Join("\n", sortedRecipes.Select(r => r.Name));

            string recipeName = Microsoft.VisualBasic.Interaction.InputBox($"Recipe List:\n{recipeNames}\n\nEnter the name of the recipe to view:", "Recipe List");
            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name == recipeName);

            if (selectedRecipe != null)
            {
                string ingredients = string.Join("\n", selectedRecipe.Ingredients.Select(i => $"{i.Name} ({i.Calories} calories, {i.FoodGroup})"));
                string steps = string.Join("\n", selectedRecipe.Steps);
                int totalCalories = selectedRecipe.Ingredients.Sum(i => i.Calories);
                string message = $"Recipe: {selectedRecipe.Name}\n\nIngredients:\n{ingredients}\n\nSteps:\n{steps}\n\nTotal Calories: {totalCalories}";

                if (totalCalories > 300)
                    message += "\nThis recipe exceeds 300 calories";

                MessageBox.Show(message, "Recipe Details");
            }
            else
            {
                MessageBox.Show("Recipe not found", "Recipe List");
            }
        }

        private void FilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            string filterOption = Microsoft.VisualBasic.Interaction.InputBox("Select a filter option:\n1. Filter by Ingredient\n2. Filter by Food Group\n3. Filter by Maximum Calories", "Filter Recipes");

            switch (filterOption)
            {
                case "1":
                    string ingredientFilter = Microsoft.VisualBasic.Interaction.InputBox("Enter an ingredient to filter by:", "Filter by Ingredient");
                    if (!string.IsNullOrEmpty(ingredientFilter))
                    {
                        var filteredRecipes = recipes.Where(r => r.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientFilter.ToLower())));
                        UpdateRecipesListView(filteredRecipes);
                    }
                    break;

                case "2":
                    string foodGroupFilter = Microsoft.VisualBasic.Interaction.InputBox("Enter a food group to filter by:", "Filter by Food Group");
                    if (!string.IsNullOrEmpty(foodGroupFilter))
                    {
                        var filteredRecipes = recipes.Where(r => r.Ingredients.Any(i => i.FoodGroup.ToLower() == foodGroupFilter.ToLower()));
                        UpdateRecipesListView(filteredRecipes);
                    }
                    break;

                case "3":
                    string maxCaloriesFilter = Microsoft.VisualBasic.Interaction.InputBox("Enter the maximum number of calories to filter by:", "Filter by Maximum Calories");
                    if (!string.IsNullOrEmpty(maxCaloriesFilter) && int.TryParse(maxCaloriesFilter, out int maxCalories))
                    {
                        var filteredRecipes = recipes.Where(r => r.Ingredients.Sum(i => i.Calories) <= maxCalories);
                        UpdateRecipesListView(filteredRecipes);
                    }
                    break;

                default:
                    MessageBox.Show("Invalid filter option.", "Filter Recipes");
                    break;
            }
        }

        private void UpdateRecipesListView(IEnumerable<Recipe> filteredRecipes = null)
        {
            recipesListView.ItemsSource = null;

            if (filteredRecipes != null)
                recipesListView.ItemsSource = filteredRecipes;
            else
                recipesListView.ItemsSource = recipes;
        }


        private void ClearInputFields()
        {
            recipeNameTextBox.Text = string.Empty;
            ingredientsListBox.Items.Clear();
            stepsListBox.Items.Clear();
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
       

    }
}

