class SearchController < ApplicationController
  def index
  end
  
  def result
    user_id = 0
    if (params[:fridge])
      user_id = current_user.id
    end    
    
    unless (params[:query].empty?)
      @results = Recipe.paginatedSearch(params[:query], params[:page], "search", user_id, @results)
    end
    
    unless (params[:queryNames].empty?)
      @results = Recipe.paginatedSearch(params[:queryNames], params[:page], "searchByName", user_id, @results)
    end
    
    unless (params[:queryIngredients].empty?)
      @results = Recipe.paginatedSearch(params[:queryIngredients], params[:page], "searchByIngredients", user_id, @results)
    end
    
    if (@results.nil? and params[:fridge])
      @results = Recipe.paginatedAllSorted(params[:page], user_id)
    end
  end
end
