class Product < ActiveRecord::Base
  validates_presence_of :name
  
  def nameWithoutSpaces
    name.gsub(/ /, "_")
  end
end
