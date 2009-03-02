class CreateFavouriteRecipes < ActiveRecord::Migration
  def self.up
    create_table :favourite_recipes do |t|
      t.column :user_id, :integer, :null => false
      t.column :recipe_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :favourite_recipes
  end
end
