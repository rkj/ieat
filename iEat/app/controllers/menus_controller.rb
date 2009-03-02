class MenusController < ApplicationController
  def list
    @selectedRecipes = current_user.selectedRecipes
  end
  
  def addRecipe
    recipe = SelectedRecipe.find(:all, :conditions => ["user_id = ? AND recipe_id = ?", current_user.id, params[:id]])[0]
    SelectedRecipe.create(:user_id => current_user.id, :recipe_id => params[:id]) if recipe.nil?
    render :text => ""
  end
  
  def removeRecipe
    print params[:id]
    SelectedRecipe.find(:all, :conditions => ["user_id = ? AND recipe_id = ?", current_user.id, params[:id]])[0].destroy
    render :text => ""
  end
  
  def shoppingList
    arrayofhashes = ActiveRecord::Base.connection.select_all("SELECT products.name product_name, units.name unit_name, SUM(step_ingredients.amount) amount 
      FROM selected_recipes, recipes, recipe_steps, step_ingredients, products, units 
      WHERE selected_recipes.user_id = #{current_user.id} AND 
            selected_recipes.recipe_id = recipes.id AND 
            recipe_steps.recipe_id = recipes.id AND 
            step_ingredients.recipe_step_id = recipe_steps.id AND 
            products.id = step_ingredients.product_id AND 
            units.id = step_ingredients.unit_id 
      GROUP BY products.name, units.name 
      ORDER BY products.name, units.name;")
    @print = false
    @products = []
    @amounts = {}
    arrayofhashes.each do |item|
      name = item["product_name"]
      if (@products.include? name)
        @amounts[name] << [item["amount"], item["unit_name"]]
      else
        @products << name
        @amounts[name] = []
        @amounts[name] << [item["amount"], item["unit_name"]]
      end
    end
  end
  
  def printableShoppingList
    shoppingList
    @print = true
    render :template => "menus/shoppingList", :layout => "empty"
  end
end
