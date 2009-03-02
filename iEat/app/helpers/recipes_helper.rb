module RecipesHelper
  def indexOfRecipeStep(recipe_step)
    @recipe.recipe_steps.index(recipe_step)
  end
  
  def sortableSteps()
    return sortable_element('recipe-edit-list', :url => { :action => "order" }) 
  end
 
  def isFavourited(recipe)
    current_user.favourites.include? recipe
  end
end
