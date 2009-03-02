class Container < ActiveRecord::Base
  has_many :container_products
  has_many :products, :through => :container_products
  belongs_to :owner, :class_name => "User", :foreign_key => :owner_id
  belongs_to :container_type
  
  validates_presence_of :name, :container_type
end
