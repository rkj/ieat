using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace IEatMobile
{
    [XmlRoot("recipe")]
    public class Recipe
    {
        private int id;
        private string name;
        private int ownerId;
        private decimal numberOfServings;
        private decimal minutesToPrepare;
        private int editingUserId;
        private string description;
        private List<RecipeStep> steps;

        public void Deserialize(XmlReader xmlr)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(Recipe));
            Recipe r = (Recipe)xmls.Deserialize(xmlr);
            this.name = r.name;
            this.ownerId = r.ownerId;
            this.numberOfServings = r.numberOfServings;
            this.minutesToPrepare = r.minutesToPrepare;
            this.editingUserId = r.editingUserId;
            this.description = r.description;
            this.id = r.id;
            Steps = r.Steps;
        }

        public void Serialize(XmlWriter xmlw)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Recipe));
            xs.Serialize(xmlw, this);
        }
        [XmlElement("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [XmlElement("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlElement("owner_id")]
        public int OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }

        [XmlElement("number_of_servings")]
        public decimal NumberOfServings
        {
            get { return numberOfServings; }
            set { numberOfServings = value; }
        }

        [XmlElement("minutes_to_prepare")]
        public decimal MinutesToPrepare
        {
            get { return minutesToPrepare; }
            set { minutesToPrepare = value; }
        }

        [XmlElement("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [XmlArrayItem("recipe_step", typeof(RecipeStep))]
        [XmlArray("recipe_steps")]
        public List<RecipeStep> Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        [XmlElement("editing_user_id")]
        private int EditingUserId
        {
            get { return 0; }
            set { }
        }
    }
}
