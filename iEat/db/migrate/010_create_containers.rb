class CreateContainers < ActiveRecord::Migration
  def self.up
    create_table :containers do |t|
      t.column :container_type_id, :integer, :null => false
      t.column :owner_id, :integer, :null => false, :references => :users
      t.column :name, :string
    end
  end

  def self.down
    drop_table :containers
  end
end