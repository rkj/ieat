class CreateContainerProducts < ActiveRecord::Migration
  def self.up
    create_table :container_products do |t|
      t.column :container_id, :integer, :null => false
      t.column :product_id, :integer, :null => false
      t.column :unit_id, :integer, :null => false
      t.column :amount, :decimal, :null => false, :scale => 3
      t.column :expiration_date, :date
    end
  end

  def self.down
    drop_table :container_products
  end
end
