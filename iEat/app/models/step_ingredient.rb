class StepIngredient < ActiveRecord::Base
  belongs_to :unit
  belongs_to :product
  belongs_to :recipe_step
  validates_numericality_of :amount
end
