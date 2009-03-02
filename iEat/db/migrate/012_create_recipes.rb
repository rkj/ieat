class CreateRecipes < ActiveRecord::Migration
  def self.up
    create_table :recipes do |t|
      t.column :name, :string, :null => false
      t.column :owner_id, :integer, :null => false, :references => :users
      t.column :number_of_servings, :integer
      t.column :minutes_to_prepare, :integer
      t.column :description, :text
      t.column :editing_user_id, :integer, :references => :users
    end
  end

  def self.down
    drop_table :recipes
  end
end
