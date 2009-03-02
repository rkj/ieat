class AddSubjectToUserMessages < ActiveRecord::Migration
  def self.up
    add_column :user_messages, :subject, :string
  end

  def self.down
    drop_column :user_messages, :subject
  end
end
