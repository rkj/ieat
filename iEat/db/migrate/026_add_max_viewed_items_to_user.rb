class AddMaxViewedItemsToUser < ActiveRecord::Migration
  def self.up
    add_column :users, :max_viewed_items, :integer, :default => 5
  end

  def self.down
    drop_column :users, :max_viewed_items
  end
end
