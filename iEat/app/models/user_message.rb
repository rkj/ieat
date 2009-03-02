class UserMessage < ActiveRecord::Base
  belongs_to :author, :class_name => "User", :foreign_key => :author_id
  belongs_to :addressee, :class_name => "User", :foreign_key => :addressee_id
  belongs_to :message_type
end
