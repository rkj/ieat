class CreateRecipeComments < ActiveRecord::Migration
  def self.up
    create_table :recipe_comments do |t|
      t.column :user_id, :integer, :null => false
      t.column :recipe_id, :integer, :null => false
      t.column :comment, :text, :null => false
    end
  end

  def self.down
    drop_table :recipe_comments
  end
end
