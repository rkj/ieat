class AddDeletedStatesToUserMessages < ActiveRecord::Migration
  def self.up
    add_column :user_messages, :author_deleted, :boolean, :default => 0
    add_column :user_messages, :addressee_deleted, :boolean, :default => 0
  end

  def self.down
    drop_column :user_messages, :author_deleted
    drop_column :user_messages, :addressee_deleted
  end
end
