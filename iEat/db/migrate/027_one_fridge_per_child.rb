class OneFridgePerChild < ActiveRecord::Migration
  def self.up
		User.find(:all).each do |u|
			if u.containers[0].nil?
				container_type = ContainerType.find_by_name('Refrigerator')
				container = Container.create(:owner => u, :name => 'Default Refrigerator', :container_type => container_type)
			end
		end
  end

  def self.down
  end
end
