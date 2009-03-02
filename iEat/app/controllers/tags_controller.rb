class TagsController < ApplicationController
  def index
    @tags = Tag.tags(:limit => 100, :order => "name desc")
  end
  
  def show
    @tag = Tag.find_by_name(params[:id])
    @recipes = []
    @tag.taggings.each do |tagging|
      @recipes << tagging.taggable
    end
  end
end
