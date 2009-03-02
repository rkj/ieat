class CreateContainerTypes < ActiveRecord::Migration
  def self.up
    create_table :container_types do |t|
      t.column :name, :string, :null => false
    end
  end

  def self.down
    drop_table :container_types
  end
end
