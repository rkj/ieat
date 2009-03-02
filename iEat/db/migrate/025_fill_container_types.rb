class FillContainerTypes < ActiveRecord::Migration
  def self.up
    ContainerType.create(:name => "Refrigerator")
    ContainerType.create(:name => "Medicine Chest")
    ContainerType.create(:name => "Bar")
  end

  def self.down
    ContainerType.delete_all
  end
end
