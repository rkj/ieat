class CreateDislikedProducts < ActiveRecord::Migration
  def self.up
    create_table :disliked_products do |t|
      t.column :user_id, :integer, :null => false
      t.column :product_id, :integer, :null => false
    end
  end

  def self.down
    drop_table :disliked_products
  end
end
