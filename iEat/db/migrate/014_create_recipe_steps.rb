class CreateRecipeSteps < ActiveRecord::Migration
  def self.up
    create_table :recipe_steps do |t|
      t.column :recipe_id, :integer, :null => false
      t.column :step_order_no, :integer, :null => false
      t.column :name, :string
      t.column :description, :text, :null => false
    end
  end

  def self.down
    drop_table :recipe_steps
  end
end
