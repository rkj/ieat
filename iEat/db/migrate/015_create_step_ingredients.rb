class CreateStepIngredients < ActiveRecord::Migration
  def self.up
    create_table :step_ingredients do |t|
      t.column :product_id, :integer, :null => false
      t.column :unit_id, :integer, :null => false
      t.column :amount, :decimal, :null => false, :scale => 3
      t.column :recipe_step_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :step_ingredients
  end
end
