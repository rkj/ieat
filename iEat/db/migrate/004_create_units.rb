class CreateUnits < ActiveRecord::Migration
  def self.up
    create_table :units do |t|
      t.column :name, :string, :null => false
      t.column :name_plural, :string
      t.column :name_genitive, :string
    end
  end

  def self.down
    drop_table :units
  end
end
