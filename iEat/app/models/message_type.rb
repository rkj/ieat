class MessageType < ActiveRecord::Base
  belongs_to :user_message
  
  def self.inheritance_column
    "dupa"
  end
end
