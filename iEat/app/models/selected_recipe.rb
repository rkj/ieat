class SelectedRecipe < ActiveRecord::Base
  belongs_to :user
  belongs_to :recipe
end
