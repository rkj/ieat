class CreateSelectedRecipes < ActiveRecord::Migration
  def self.up
    create_table :selected_recipes do |t|
      t.column :user_id, :integer, :null => false
      t.column :recipe_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :selected_recipes
  end
end
