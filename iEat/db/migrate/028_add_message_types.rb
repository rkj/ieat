class AddMessageTypes < ActiveRecord::Migration
  def self.up
    execute "INSERT INTO message_types VALUES(1, 'Normal')"
    execute "INSERT INTO message_types VALUES(2, 'Dinner invitation')"
  end

  def self.down
    execute "DELETE FROM message_types"
  end
end
