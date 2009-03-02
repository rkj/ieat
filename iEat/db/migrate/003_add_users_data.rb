class AddUsersData < ActiveRecord::Migration
  def self.up
    add_column :users, :first_name, :string
    add_column :users, :last_name, :string
    add_column :users, :date_of_birth, :date
    add_column :users, :sex, :boolean
    add_column :users, :avatar, :string
  end

  def self.down
    remove_column :users, :first_name
    remove_column :users, :last_name
    remove_column :users, :date_of_birth
    remove_column :users, :sex
    remove_column :users, :avatar
  end
end
