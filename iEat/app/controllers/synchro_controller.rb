class SynchroController < ApplicationController
  layout false
  
  skip_filter :login_required
  def recipe
    @recipe = Recipe.find(params[:id])
  end
  
  skip_filter :login_required
  def recipes_list
    @recipes = Recipe.find(:all)
  end
  
  skip_filter :login_required
  def products_list
    @products = Product.find(:all)
  end
end
