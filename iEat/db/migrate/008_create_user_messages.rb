class CreateUserMessages < ActiveRecord::Migration
  def self.up
    create_table :user_messages do |t|
      t.column :author_id, :integer, :null => false, :references => :users
      t.column :addressee_id, :integer, :null => false, :references => :users
      t.column :message_type_id, :integer, :null => false
      t.column :message, :text, :null => false
    end
  end

  def self.down
    drop_table :user_messages
  end
end
