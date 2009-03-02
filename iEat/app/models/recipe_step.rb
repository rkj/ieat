class RecipeStep < ActiveRecord::Base
  has_many :step_ingredients
  belongs_to :recipe
end
