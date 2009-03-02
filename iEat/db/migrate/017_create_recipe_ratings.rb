class CreateRecipeRatings < ActiveRecord::Migration
  def self.up
    create_table :recipe_ratings do |t|
      t.column :user_id, :integer, :null => false
      t.column :recipe_id, :integer, :null => false
      t.column :rating, :integer, :null => false
    end
  end

  def self.down
    drop_table :recipe_ratings
  end
end
