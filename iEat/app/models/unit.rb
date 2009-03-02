class Unit < ActiveRecord::Base
  validates_presence_of :name
  
  def nameWithCount(count)
    #TODO
    name
  end
end
