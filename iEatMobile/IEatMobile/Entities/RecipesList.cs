using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace IEatMobile
{
    [XmlRoot("recipesList")]
    public class RecipesList
    {
        private RecipeForList[] listedRecipies;

        [XmlArrayItem("recipe", typeof(RecipeForList))]
        [XmlArray("recipes")]
        public RecipeForList[] ListedRecipies
        {
            get { return listedRecipies; }
            set { listedRecipies = value; }
        }
        private List<Recipe> recipes;
        [XmlIgnore]
        public List<Recipe> Recipes
        {
            get { return recipes; }
            set { recipes = value; }
        }
        public Recipe getRecipeById(int id)
        {
            foreach (Recipe r in recipes)
                if (r.Id == id)
                    return r;
            return null;
        }
        public void Serialize(System.IO.Stream oStream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RecipesList));
            xs.Serialize(oStream,this);
        }
        public void Serialize(System.Xml.XmlWriter xmlw)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RecipesList));
            xs.Serialize(xmlw, this);
        }
        public void Deserialize(System.Xml.XmlReader xmlr)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RecipesList));
            RecipesList rl = (RecipesList)xs.Deserialize(xmlr);
            this.ListedRecipies = rl.ListedRecipies; 
        }
        public void Deserialize(System.IO.Stream os)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RecipesList));
            RecipesList rl = (RecipesList)xs.Deserialize(os);
            this.ListedRecipies = rl.ListedRecipies;
        }
        public RecipesList()
        {
        }
       
        public void fillWithData()
        {
            /*
            recipes = new List<Recipe>();

            Recipe recipe = new Recipe();
            recipe.MinutesToPrepare = 10;
            recipe.Name = "Ciasto truskawskowe";
            recipe.Description = "Ciasto to jest bardzo dobre";
            recipe.NumberOfServings = 3;
            recipe.OwnerId = 1;
            recipe.Steps = new List<RecipeStep>();

            RecipeStep step = new RecipeStep();
            step.Description = "Robimy i pieczemy biszkopt";
            step.Name = "Biszkopt";
            step.Step_order_no = 1;
            step.Ingredients = new List<StepIngredient>();
            StepIngredient ingrid = new StepIngredient();
            ingrid.Amount = 10;
            step.Ingredients.Add(ingrid);
            recipe.Steps.Add(step);
            
            step = new RecipeStep();
            step.Description = "Robimy galaretke";
            step.Name = "Galaretka";
            step.Step_order_no = 2;
            step.Ingredients = new List<StepIngredient>();
            ingrid = new StepIngredient();
            ingrid.Amount = 10;
            step.Ingredients.Add(ingrid);
            recipe.Steps.Add(step);

            step = new RecipeStep();
            step.Description = "Zanurzamy truskawki";
            step.Name = "Truskawki";
            step.Step_order_no = 2;
            step.Ingredients = new List<StepIngredient>();
            ingrid = new StepIngredient();
            ingrid.Amount = 10;
            step.Ingredients.Add(ingrid);
            recipe.Steps.Add(step);

            recipes.Add(recipe);
             */ 
        }
    }
}
