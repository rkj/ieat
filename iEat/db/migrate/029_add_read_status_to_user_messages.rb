class AddReadStatusToUserMessages < ActiveRecord::Migration
  def self.up
    add_column :user_messages, :read, :boolean, :default => 0
  end

  def self.down
    drop_column :user_messages, :read
  end
end
