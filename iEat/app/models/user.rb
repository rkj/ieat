require 'digest/sha1'
class User < ActiveRecord::Base
  has_many :disliked_products
  has_many :dislikedProducts, :source => :product, :through => :disliked_products
  has_many :favourite_recipes
  has_many :favourites, :source => :recipe, :through => :favourite_recipes, :order => "favourite_recipes.ID DESC"
  has_many :friendships
  has_many :friends, :through => :friendships, :source => :friend
  has_many :containers, :foreign_key => :owner_id
  has_many :sentMessages, :class_name => "UserMessage", :foreign_key => :author_id
  has_many :receivedMessages, :class_name => "UserMessage", :foreign_key => :addressee_id
  has_many :selected_recipes
  has_many :selectedRecipes, :source => :recipe, :through => :selected_recipes, :order => "selected_recipes.ID DESC"
  has_many :editingRecipes, :source => :recipe, :class_name => "Recipe", :foreign_key => 'editing_user_id'
  has_many :awaitings, :class_name => "Friendship", :finder_sql => <<-'END'
    SELECT friendships.* 
    FROM friendships WHERE friendships.friend_id = #{id}
    AND coalesce(friendships.accepted, 0) = 0 
  END
  
  # Virtual attribute for the unencrypted password
  attr_accessor :password

  validates_presence_of     :login, :email
  validates_presence_of     :password,                   :if => :password_required?
  validates_presence_of     :password_confirmation,      :if => :password_required?
  validates_length_of       :password, :within => 4..40, :if => :password_required?
  validates_confirmation_of :password,                   :if => :password_required?
  validates_length_of       :login,    :within => 3..40
  validates_length_of       :email,    :within => 3..100
  validates_uniqueness_of   :login, :email, :case_sensitive => false
  before_save :encrypt_password
  
  # prevents a user from submitting a crafted form that bypasses activation
  # anything else you want your user to change should be added here.
  attr_accessible :login, :email, :password, :password_confirmation
  
  # Authenticates a user by their login name and unencrypted password.  Returns the user or nil.
  def self.authenticate(login, password)
    u = find_by_login(login) # need to get the salt
    u && u.authenticated?(password) ? u : nil
  end

  # Encrypts some data with the salt.
  def self.encrypt(password, salt)
    Digest::SHA1.hexdigest("--#{salt}--#{password}--")
  end

  # Encrypts the password with the user salt
  def encrypt(password)
    self.class.encrypt(password, salt)
  end

  def authenticated?(password)
    crypted_password == encrypt(password)
  end

  def remember_token?
    remember_token_expires_at && Time.now.utc < remember_token_expires_at 
  end

  # These create and unset the fields required for remembering users between browser closes
  def remember_me
    remember_me_for 2.weeks
  end

  def remember_me_for(time)
    remember_me_until time.from_now.utc
  end

  def remember_me_until(time)
    self.remember_token_expires_at = time
    self.remember_token            = encrypt("#{email}--#{remember_token_expires_at}")
    save(false)
  end

  def forget_me
    self.remember_token_expires_at = nil
    self.remember_token            = nil
    save(false)
  end
  
  def change_password(old_passwd, new_passwd, new_passwd2)
    return if old_passwd.empty?
    # if (new_passwd2 != new_passwd)
    #   self.errors.add(:password, "is not repeated correctly!")
    #   return
    # end
    if (encrypt(old_passwd) != self.crypted_password)
      self.errors.add(:old_password, "not correct!")
      return
    end
    self.password = new_passwd
    self.password_confirmation = new_passwd2
    save
  end
  
  def confirmFriendship(friendship)
    friendship.accepted = true
    friendship.save!
    Friendship.create!(:user_id => self.id, :friend_id => friendship.user_id, :accepted => true)
  end
  
  def unreadMessagesCount
    UserMessage.find(:all, :conditions => ["addressee_id = ? AND read = 'f' AND addressee_deleted = 'f' AND message_type_id = 1", self.id]).length
  end
  
  def unreadDinnerInvitationsCount
    UserMessage.find(:all, :conditions => ["addressee_id = ? AND read = 'f' AND addressee_deleted = 'f' AND message_type_id = 2", self.id]).length
  end
  
  def unreadFriendInvitationsCount
    UserMessage.find(:all, :conditions => ["addressee_id = ? AND read = 'f' AND addressee_deleted = 'f' AND message_type_id = 3", self.id]).length
  end
  
  def hasProduct?(name)
    product = Product.find_by_name(name)
    self.containers[0].products.include? product
  end
  
  def isDislikedProduct?(name)
    product = Product.find_by_name(name)
    self.dislikedProducts.include? product
  end
  
  protected
    # before filter 
    def encrypt_password
      return if password.blank?
      self.salt = Digest::SHA1.hexdigest("--#{Time.now.to_s}--#{login}--") if new_record?
      self.crypted_password = encrypt(password)
    end
    
    def password_required?
      crypted_password.blank? || !password.blank?
    end
    
end
