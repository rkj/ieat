require 'xml_markup_addon'
class RecipesController < ApplicationController  
  auto_complete_for :unit, :name
  auto_complete_for :product, :name
  skip_before_filter :verify_authenticity_token, :only => [:auto_complete_for_unit_name, :auto_complete_for_product_name]
  
  def order
    params["recipe-edit-list"].each_with_index { |id,idx| RecipeStep.update(id, :step_order_no => idx+1) }
  end
  
  def index
    redirect_to :action => :list
  end
    
  def search
    @recipes = Recipe.paginatedSearch(params[:query], params[:page])
    render :action => :list
  end
  
  def show
    if params[:id]
      @recipe = Recipe.find_by_id(params[:id])
      @average = @recipe.averageRating
      @your = @recipe.ratingFromUser(current_user.id)
    else
      flash[:notice] = "You have to provide ID"
      redirect_to :action => :list
    end
  end
  
  def addComment
    @recipe = Recipe.find_by_id(params[:id])
    comment = []
    comment << @recipe.recipe_comments.create(:user_id => current_user.id, :comment => params[:comment])
    render :partial => "show_comment", :collection => comment
  end
  
  def edit
    if params[:recipe]
      @recipe = Recipe.find_by_id(params[:id])
      params[:recipe].each { |kv| @recipe[kv[0]] = kv[1] unless kv[0] == :tag_list}
      tagList = params[:recipe][:tag_list]
      @recipe.tag_list = []
      tagList.split(/[, ]/).each do |tag|
        if (tag.length > 0)
          @recipe.tag_list << tag
        end 
      end
      @recipe.save
    else
      @recipe = Recipe.find_by_id(params[:id]) || Recipe.new
      unless @recipe.editing_user.nil? or @recipe.editing_user == current_user
        flash[:notice] = "Someone else is already editing recipe '#{@recipe.name}'!"
        redirect_to :action => :list
      end
      @recipe.editing_user = current_user
      @recipe.save!
    end
  end
  
  def list
    @recipes = Recipe.paginate :all, :conditions => { :editing_user_id => nil }, :page => params[:page], :per_page => 15
  end
  
  def addStepIngredient
    begin
      @step = RecipeStep.find_by_id(params[:id])
      @step_ingredient = createIngridient(params[:unit], params[:product], params[:amount])
      @step.step_ingredients << @step_ingredient
      @step.save!
      if request.xml_http_request?
        render(:partial => 'step_ingredient', :layout => false) and return
      end
    rescue => e
      if request.xml_http_request?
        err = "Unit's " + @step_ingredient.unit.errors.full_messages.join("<br />") + "<br />Product's " + @step_ingredient.product.errors.full_messages.join("<br />") + "<br />" + @step_ingredient.errors.full_messages.join("<br />")
        render :text => err, :status => 444 and return
      else
        flash[:notice] = "Unable to add ingredient."
      end
    end
    redirect_to :action => :edit, :id => @step.recipe_id
  end
  
  def addRecipeStep
    @recipe = Recipe.find_by_id(params[:id])
    @recipe_step = @recipe.recipe_steps.create(:step_order_no => @recipe.recipe_steps.length + 1)
    render :partial => "recipe_step"
  end
  
  def deleteRecipeStep
    rs = RecipeStep.find_by_id(params[:id])
    order = rs.step_order_no
    recipe = rs.recipe
    recipe.recipe_steps.each do |step|
      if (step.step_order_no > order)
        step.step_order_no -= 1
        step.save!
      end
    end
    rs.destroy
    recipe.reload
    render :partial => "recipe_step", :collection => recipe.recipe_steps
  end
  
  def saveRecipeStep
    rs = RecipeStep.find_by_id(params[:id])
    rs.update_attributes!(params[:recipe_step])
    if request.xml_http_request?
      render :text => "" 
    else
      redirect_to :action => :edit, :id => rs.recipe.id
    end
  end
  
  def new
    if params[:recipe]
      @recipe = Recipe.new(params[:recipe])
      @recipe.editing_user_id = current_user.id
      @recipe.owner_id = current_user.id
      if (@recipe.save)
        @recipe.recipe_steps.create(:step_order_no => 1)
        return redirect_to(:action => :edit, :id => @recipe)
      end
    else 
      @recipe = Recipe.new
    end
    render :action => :edit
  end

  def endEditingRecipe
    @recipe = Recipe.find_by_id(params[:id])
    @recipe.editing_user = nil
    @recipe.save!
    unless request.xhr?
      flash[:notice] = "Recipe saved"
      redirect_to :action => :show, :id => @recipe.id
    end
  end
  
  def deleteStepIngredient
    si = StepIngredient.find_by_id(params[:id])
    recipe_id = si.recipe_step.recipe.id
    si.destroy
    if request.xml_http_request?
      render :text => "Wpis usunięty"
    else
      redirect_to :action => :edit, :id => recipe_id
    end
  end
  
  include RecipesHelper
  def addToFavourites
    @recipe = Recipe.find_by_id(params[:id])
    if (isFavourited(@recipe))
      return render(:text => "Recipe already in favourities!")
    end
    current_user.favourites << @recipe
    redirect_to :action => :list unless request.xhr?
  end
  
  def removeFromFavourites
    @recipe = Recipe.find_by_id(params[:id])
    current_user.favourites.delete @recipe
    redirect_to :action => :list unless request.xhr?
  end
  
  def rate
    @recipe = Recipe.find_by_id(params[:id])
    @your = @recipe.ratingFromUser(current_user.id)
    unless @your
      @your = params[:rating].to_i
      @recipe.recipe_ratings.create(:user_id => current_user.id, :rating => @your)
    end
    @average = @recipe.averageRating
    render :partial => "ratings"
  end
  
  private
    def createIngridient(unit, product, amount)
      unit = Unit.find_or_create_by_name(unit[:name])
      product = Product.find_or_create_by_name(product[:name])
      ingredient = StepIngredient.new(:amount => amount)
      ingredient.product = product
      ingredient.unit = unit
      return ingredient
    end
end
