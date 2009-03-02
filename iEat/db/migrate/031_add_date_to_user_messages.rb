class AddDateToUserMessages < ActiveRecord::Migration
  def self.up
    add_column :user_messages, :date_created, :datetime, :default => "timestamp"
  end

  def self.down
    drop_column :user_messages, :date_created
  end
end
