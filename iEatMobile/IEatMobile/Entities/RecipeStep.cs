using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IEatMobile
{
    public class RecipeStep
    {
        private int id;
        private int recipeId;
        private string name;
        private string description;
        private int step_order_no;
        private List<StepIngredient> ingredients;

        [XmlElement("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlElement("recipe_id")]
        public int RecipeId
        {
            get { return recipeId; }
            set { recipeId = value; }
        }

        [XmlElement("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlElement("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [XmlElement("step_order_no")]
        public int Step_order_no
        {
            get { return step_order_no; }
            set { step_order_no = value; }
        }
        
        [XmlArrayItem("step_ingredient", typeof(StepIngredient))]
        [XmlArray("step_ingredients")]
        public List<StepIngredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }
    }
}
