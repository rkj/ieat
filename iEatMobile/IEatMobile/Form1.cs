using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace IEatMobile
{
    public partial class Form1 : Form
    {
        private DataManager dataManager;
        private Recipe newRecipe;
        private int prev;

        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private System.Windows.Forms.ToolBarButton toolBarButton3;

        private List<List<ToolBarButton>> toolbars = new List<List<ToolBarButton>>();

        public Form1()
        {
            InitializeComponent();
            
           
            dataManager = new DataManager();
            try
            {
                dataManager.loadProducts();
                dataManager.loadRecipesList();
                dataManager.loadRecipes();
                dataManager.loadFridges();
                dataManager.loadFridge();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message+"\nTrying to download from server...");
                try
                {
                    dataManager.getProductsList();
                    dataManager.getRecipesList();
                    dataManager.getFridgeList();
                    dataManager.getFridge();
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }
            }
           
            //dataManager.getProductsList();
            //dataManager.getRecipesList();
           // dataManager.fillWithTestData();
         
            this.recipeBindingSource.DataSource = dataManager.RecipesList.Recipes;
            this.fridgeProductBindingSource.DataSource = dataManager.Fridge;

            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1.ImageIndex = 0;

            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2.ImageIndex = 1;

            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3.ImageIndex = 2;

            List<ToolBarButton> tempList = new List<ToolBarButton>();
            tempList.Add(toolBarButton1);
            toolbars.Add(tempList);

            tempList = new List<ToolBarButton>();
            toolbars.Add(tempList);

            tempList = new List<ToolBarButton>();
            tempList.Add(toolBarButton2);
            tempList.Add(toolBarButton3);
            toolbars.Add(tempList);
           
            tempList = new List<ToolBarButton>();
            toolbars.Add(tempList);

            foreach (ToolBarButton button in toolbars[tabcontrolMain.SelectedIndex])
            {
                toolBar1.Buttons.Add(button);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolBar1.Buttons.Clear();
            foreach (ToolBarButton button in toolbars[tabcontrolMain.SelectedIndex])
            {
                toolBar1.Buttons.Add(button);
            }
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            MessageBox.Show(e.Button.ImageIndex + "");
        }

        private void productTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (productTabControl.TabPages[productTabControl.SelectedIndex].Equals(productInfo))
            {
                if (productDataGrid.CurrentRowIndex < 0 || productDataGrid.CurrentRowIndex >= dataManager.Fridge.Count)
                {
                    productInfo.Hide();
                }
                else
                {
                    FridgeProduct fProduct = dataManager.Fridge[productDataGrid.CurrentRowIndex];
                    productInfoDescription.Text = fProduct.Description;
                    productInfoName.Text = fProduct.Name;
                    productsInfoExpire.Value = fProduct.ExpireDate;
                    productInfoQuantity.Text = fProduct.Amount + "";
                    productInfo.Show();
                }
            }
            else if (productTabControl.TabPages[productTabControl.SelectedIndex].Equals(productNew))
            {
                listBox.Items.Clear();
                foreach (Product p in dataManager.ProductsList.Products)
                    listBox.Items.Add(p.Name);
                addButton.Enabled = false;
            }
        }

        private void RecipesTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecipesTabControl.TabPages[RecipesTabControl.SelectedIndex].Equals(RecipeInfo))
            {
                if (recipesDataGrid.CurrentRowIndex < 0 || recipesDataGrid.CurrentRowIndex >= dataManager.RecipesList.Recipes.Count)
                {
                    RecipeInfo.Hide();
                }
                else
                {
                    Recipe recipe = dataManager.RecipesList.Recipes[recipesDataGrid.CurrentRowIndex];
                    recipesInfoNumSteps.Text = recipe.Steps.Count + "";
                    recipesInfoSteps.Maximum = recipe.Steps.Count;
                    recipesInfoSteps.Value = 0;

                    recipesInfoStepDescription.Text = recipe.Description;
                    recipesInfoStepName.Text = recipe.Name;
                    recipesInfoServVal.Text = recipe.NumberOfServings + "";
                    recipesInfoTimeVal.Text = recipe.MinutesToPrepare + "";

                    recipesInfoServ.Show();
                    recipesInfoServVal.Show();
                    recipesInfoTime.Show();
                    recipesInfoTimeVal.Show();
                    recipesInfoStepIngredients.Hide();
                    recipesInfoIngridLabel.Hide();

                    RecipeInfo.Show();
                }
            }
            else if (RecipesTabControl.TabPages[RecipesTabControl.SelectedIndex].Equals(recipesNew))
            {
                newRecipe = new Recipe();
                newRecipe.Steps = new List<RecipeStep>();

                recipesNewSteps.Value = 0;
                recipesNewSteps.Maximum = 0;
                prev = 0;
                recipesNewStepsCount.Text = recipesNewSteps.Maximum+"";

                recipesNewServings.Show();
                recipesNewServingsVal.Show();
                recipesNewTime.Show();
                recipesNewTimeVal.Show();
                recipesNewStepIngredients.Hide();
                recipesNewStepIngridLabel.Hide();
            }
        }

        private void recipesInfoSteps_ValueChanged(object sender, EventArgs e)
        {
            if (recipesDataGrid.CurrentRowIndex < 0 || recipesDataGrid.CurrentRowIndex >= dataManager.RecipesList.Recipes.Count)
            {
            }
            else
            {
                Recipe recipe = dataManager.RecipesList.Recipes[recipesDataGrid.CurrentRowIndex];

                if (recipesInfoSteps.Value == 0)
                {
                    recipesInfoStepDescription.Text = recipe.Description;
                    recipesInfoStepName.Text = recipe.Name;
                    recipesInfoServVal.Text = recipe.NumberOfServings + "";
                    recipesInfoTimeVal.Text = recipe.MinutesToPrepare + "";

                    recipesInfoServ.Show();
                    recipesInfoServVal.Show();
                    recipesInfoTime.Show();
                    recipesInfoTimeVal.Show();
                    recipesInfoStepIngredients.Hide();
                    recipesInfoIngridLabel.Hide();
                }
                else
                {
                    RecipeStep step = recipe.Steps[(int)recipesInfoSteps.Value - 1];
                    recipesInfoStepDescription.Text = step.Description;
                    recipesInfoStepIngredients.Text = "";
                    foreach (StepIngredient ingrid in step.Ingredients)
                    {
                        Product p = dataManager.ProductsList.getById(ingrid.ProductId);
                        if (p == null)
                            continue;

                        recipesInfoStepIngredients.Text += p.Name + " " + ingrid.Amount + "; ";
                    }
                    recipesInfoStepName.Text = step.Name;

                    recipesInfoServ.Hide();
                    recipesInfoServVal.Hide();
                    recipesInfoTime.Hide();
                    recipesInfoTimeVal.Hide();
                    recipesInfoStepIngredients.Show();
                    recipesInfoIngridLabel.Show();
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                FridgeProduct fp = new FridgeProduct();
               
                fp.Description = productsNewDescription.Text;
                fp.Product = dataManager.ProductsList.getByName(productsNewName.Text);
                fp.Amount = float.Parse(productsNewQuantity.Text);
                fp.ExpireDate = productsNewExpire.Value;
                dataManager.Fridge.Add(fp);

                fridgeProductBindingSource.ResetBindings(false);

                productsNewName.Text = "";
                productsNewDescription.Text = "";
                productsNewQuantity.Text = "";
                productsNewExpire.Value = DateTime.Now;

                MessageBox.Show("Product added to the fridge!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured. Cannot add a new product. Please try again");
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (productDataGrid.CurrentRowIndex < 0 || productDataGrid.CurrentRowIndex >= dataManager.Fridge.Count)
                return;

            try
            {
                FridgeProduct fProduct = dataManager.Fridge[productDataGrid.CurrentRowIndex];
                fProduct.Description = productInfoDescription.Text;
                fProduct.Name = productInfoName.Text;
                fProduct.ExpireDate = productsInfoExpire.Value;
                fProduct.Amount = float.Parse(productInfoQuantity.Text);

                fridgeProductBindingSource.ResetBindings(false);
                MessageBox.Show("Product updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured. Cannot update product. Please try again");
            }
        }

        private void synchroGetPRoducts_Click(object sender, EventArgs e)
        {
            dataManager.getProductsList();
        }

        private void recipesNewSteps_ValueChanged(object sender, EventArgs e)
        {
            if (prev == 0)
            {
                newRecipe.Description = recipesNewStepDescription.Text;
                newRecipe.Name = recipesNewStepName.Text;
                try
                {
                    newRecipe.NumberOfServings = decimal.Parse(recipesNewServingsVal.Text);
                    newRecipe.MinutesToPrepare = decimal.Parse(recipesNewTimeVal.Text);
                }
                catch (Exception ee)
                {
                }
            }
            else
            {
                RecipeStep rs = newRecipe.Steps[prev - 1];
                rs.Description = recipesNewStepDescription.Text;
                rs.Name = recipesNewStepName.Text;
            }

            if (recipesNewSteps.Value == 0)
            {
                recipesNewStepDescription.Text = newRecipe.Description;
                recipesNewStepName.Text = newRecipe.Name;
                recipesNewServingsVal.Text = newRecipe.NumberOfServings + "";
                recipesNewTimeVal.Text = newRecipe.MinutesToPrepare + "";

                recipesNewServings.Show();
                recipesNewServingsVal.Show();
                recipesNewTime.Show();
                recipesNewTimeVal.Show();
                recipesNewStepIngredients.Hide();
                recipesNewStepIngridLabel.Hide();
            }
            else
            {
                RecipeStep step = newRecipe.Steps[(int)recipesNewSteps.Value - 1];
                recipesNewStepDescription.Text = step.Description;
                recipesNewStepName.Text = step.Name;
                recipesNewStepIngredients.Text = "";
                foreach (StepIngredient ingrid in step.Ingredients)
                {
                    if (ingrid.Product == null)
                        continue;

                    recipesNewStepIngredients.Text += ingrid.Product.Name + " " + ingrid.Amount + "\n";
                }

                recipesNewServings.Hide();
                recipesNewServingsVal.Hide();
                recipesNewTime.Hide();
                recipesNewTimeVal.Hide();
                recipesNewStepIngredients.Show();
                recipesNewStepIngridLabel.Show();
            }
            prev = (int)recipesNewSteps.Value;
        }

        private void recipeSyncButton_Click(object sender, EventArgs e)
        {
            //dataManager.saveProductsList();
            dataManager.getRecipesList();
        }

        private void addRecipeButton_Click(object sender, EventArgs e)
        {
            RecipeStep rs = new RecipeStep();
            rs.Ingredients = new List<StepIngredient>();
            rs.Step_order_no = (int)recipesNewSteps.Value;
            newRecipe.Steps.Add(rs);

            recipesNewSteps.Maximum = recipesNewSteps.Maximum + 1;
            recipesNewStepsCount.Text = recipesNewSteps.Maximum+"";
        }

        private void delRecipeButton_Click(object sender, EventArgs e)
        {
            if (recipesNewSteps.Value == 0)
                return;

            newRecipe.Steps.RemoveAt((int)recipesNewSteps.Value - 1);
            recipesNewSteps.Maximum = recipesNewSteps.Maximum - 1;
            recipesNewStepsCount.Text = recipesNewSteps.Maximum+"";
            int newstep=(int)Math.Min(recipesNewSteps.Maximum,recipesNewSteps.Value);
            recipesNewSteps.Value = newstep;

            if (newstep > 0)
            {
                RecipeStep rs = newRecipe.Steps[newstep - 1];
                recipesNewStepDescription.Text = rs.Description;
                recipesNewStepName.Text = rs.Name;
                recipesNewStepIngredients.Text = "";
                foreach (StepIngredient ingrid in rs.Ingredients)
                {
                    if (ingrid.Product == null)
                        continue;

                    recipesNewStepIngredients.Text += ingrid.Product.Name + " " + ingrid.Amount + "\n";
                }

                recipesNewServings.Hide();
                recipesNewServingsVal.Hide();
                recipesNewTime.Hide();
                recipesNewTimeVal.Hide();
                recipesNewStepIngredients.Show();
                recipesNewStepIngridLabel.Show();
            }
            else
            {
                recipesNewStepDescription.Text = newRecipe.Description;
                recipesNewStepName.Text = newRecipe.Name;
                recipesNewServingsVal.Text = newRecipe.NumberOfServings+"";
                recipesNewTimeVal.Text = newRecipe.MinutesToPrepare+"";

                recipesNewServings.Show();
                recipesNewServingsVal.Show();
                recipesNewTime.Show();
                recipesNewTimeVal.Show();
                recipesNewStepIngredients.Hide();
                recipesNewStepIngridLabel.Hide();
            }
        }

        private void saveRecipeButton_Click(object sender, EventArgs e)
        {
            dataManager.RecipesList.Recipes.Add(newRecipe);
            //dataManager.save();
            recipeBindingSource.ResetBindings(false);

            recipesNewStepIngredients.Text = "";
            recipesNewServingsVal.Text = "";
            recipesNewTimeVal.Text = "";
            recipesNewStepDescription.Text = "";
            recipesNewStepName.Text = "";

            MessageBox.Show("Recipe added!");

            recipesNewSteps.Value = 0;
            recipesNewSteps.Maximum = 0;
            newRecipe = new Recipe();
            newRecipe.Steps = new List<RecipeStep>();
            prev = 0;
            recipesNewStepsCount.Text = recipesNewSteps.Maximum + "";

            recipesNewServings.Show();
            recipesNewServingsVal.Show();
            recipesNewTime.Show();
            recipesNewTimeVal.Show();
            recipesNewStepIngredients.Hide();
            recipesNewStepIngridLabel.Hide();
        }

        private void productsNewName_TextChanged(object sender, EventArgs e)
        {
            if (!listBoxFocused)
            {
                listBox.Items.Clear();
                foreach (Product p in dataManager.ProductsList.Products)
                {
                    if (p.Name.Contains(productsNewName.Text))
                        listBox.Items.Add(p.Name);
                }
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            productsNewName.Text = (string)listBox.Items[listBox.SelectedIndex];
            if (listBox.SelectedIndex >= 0)
                addButton.Enabled = true;
            else
                addButton.Enabled = false;
        }
        private bool listBoxFocused;
        private void listBox_GotFocus(object sender, EventArgs e)
        {
            listBoxFocused = true;
        }

        private void listBox_LostFocus(object sender, EventArgs e)
        {
            listBoxFocused = false;
        }

        private void fridgeSyncButton_Click(object sender, EventArgs e)
        {
            dataManager.getFridgeList();
            dataManager.getFridge();
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            dataManager.saveProductsList();
            dataManager.saveRecipesList();
            dataManager.saveRecipes();
            dataManager.saveFridgesList();
            dataManager.saveFridge();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string[] words = searchText.Text.ToUpper().Split(new char[] { ' ' });
            List<Recipe> rList = new List<Recipe>();
            bool cAll;
            bool found;
            int cnt;
            foreach (Recipe r in dataManager.RecipesList.Recipes)
            {
                cnt = 0;
                for (int i=0; i<words.Length; i++)
                {
                    found = false;
                    foreach (RecipeStep s in r.Steps)
                    {
                        if (r.Name.ToUpper().Contains(words[i]))
                            found = true;
                        else
                            foreach (StepIngredient si in s.Ingredients)
                            {
                                Product p  = dataManager.ProductsList.getById(si.ProductId);
                                if (p != null && p.Name.ToUpper().Contains(words[i]))
                                    found = true;
                            }
                       
                    }
                    if (found)
                        cnt++;
                }
                if (cnt == words.Length)
                    rList.Add(r);
            }
            recipeBindingSource1.DataSource = rList;
        }
    }
}