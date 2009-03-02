class RemoveNullConstraintFromRecipeStepsDescription < ActiveRecord::Migration
  def self.up
    change_column :recipe_steps, :description, :text, :null => true
  end

  def self.down
    change_column :recipe_steps, :description, :text, :null => false
  end
end
