class ContainerProduct < ActiveRecord::Base
  belongs_to :container
  belongs_to :product
  belongs_to :unit
  validates_numericality_of :amount
end
