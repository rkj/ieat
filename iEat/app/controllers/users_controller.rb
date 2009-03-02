class UsersController < ApplicationController
  auto_complete_for :product, :name
  skip_before_filter :verify_authenticity_token, :only => [:auto_complete_for_product_name]
  skip_before_filter :login_required, :only => [:login, :do_login, :new_user]
  
  # Be sure to include AuthenticationSystem in Application Controller instead
  include AuthenticatedSystem
  def index
  end
  
  def login
  end
  
  def edit
    @user = User.find_by_id(current_user.id)
    if (params[:user])
      params[:user].each { |kv| @user[kv[0]] = kv[1] }
      @user.save!
      @user.change_password(params[:old_password], params[:new_password], params[:repeat_new_password])
    end
  end
  
  def view
    @user = User.find_by_login(params[:name]) if params[:name]
    @user = User.find_by_id(params[:id]) if params[:id]
    return render(:action => 'user_not_found') if @user.nil?
  end
  
  def do_login
    reset_session
    self.current_user = User.authenticate(params[:login], params[:password])
    if logged_in?
      if params[:remember_me] == "1"
        self.current_user.remember_me
        cookies[:auth_token] = { :value => self.current_user.remember_token , :expires => self.current_user.remember_token_expires_at }
      end
      redirect_back_or_default('/')
      flash[:notice] = "Logged in successfully"
    else
      flash[:notice] = "Wrong username or password"
      redirect_to :action => 'login'
    end
  end

  def logout
    reset_session
    self.current_user.forget_me if logged_in?
    cookies.delete :auth_token
    reset_session
    flash[:notice] = "You have been logged out."
    redirect_back_or_default('/')
  end
  
  def friends
    @friendships = current_user.friendships
    @awaitings = current_user.awaitings
  end
  
  def add_friend
    @friend = User.find_by_id(params[:id])
    current_user.friends << @friend
    flash[:notice] = "#{@friend.login} added to friends"
    hishers = ""
    if (current_user.sex.nil?)
      hishers = "her/his"
    else
      if (current_user.sex)
        hishers = "his"
      else
        hishers = "her"
      end
    end
    @friend.receivedMessages.create(:author_id => current_user.id, :message_type_id => 3, 
                                    :message => "#{current_user.login} has invited you to be #{hishers} friend :)",
                                    :read => false, :subject => "Friend invitation from #{current_user.login}",
                                    :date_created => Time.now)
    redirect_to :controller => "user", :action => @friend.login
  end
  
  def acceptFriendship
    @friendship = Friendship.find_by_id(params[:id])
    @newFriendship = current_user.confirmFriendship(@friendship)
    unless request.xhr?
      flash[:notice] = "Friendship accepted"
      redirect_to :action => "friends"
    end
  end
    
  def removeFriendship
    friendship = Friendship.find_by_id params[:id]
    @id = friendship.friend.id
    friendship.destroy
    unless request.xhr?
      flash[:notice] = "Friendship removed!"
      redirect_to :action => "friends"
    end
  end
  
  def new_user
    return unless params[:user]
    cookies.delete :auth_token
    # protects against session fixation attacks, wreaks havoc with 
    # request forgery protection.
    # uncomment at your own risk
    # reset_session
    @user = User.new(params[:user])
    @user.save!
    
    @container = Container.new(params[:container])
    @container.owner = @user
    @container.container_type = ContainerType.find_or_create_by_name('Refrigerator')
    @container.name = "Refrigerator"
    @container.save!
    
    self.current_user = @user
    redirect_back_or_default('/')
    flash[:notice] = "Thanks for signing up!"
  rescue ActiveRecord::RecordInvalid
    render :action => 'new_user'
  end
  
  def favourites
    @sort = params[:sort] || "name"
    @reverse = params[:reverse] || false
    args = [];
    if (@sort == "percentFulfilled")
      args << current_user.id
    end
    unless (@reverse) 
      @recipes = self.current_user.favourites.sort { |a, b| a.send(@sort, *args) <=> b.send(@sort, *args) }
    else 
      @recipes = self.current_user.favourites.sort { |a, b| b.send(@sort, *args) <=> a.send(@sort, *args) }
    end
  end
  
  def editedRecipes
    @recipes = current_user.editingRecipes
  end
  
  def dislikedProducts
    @dislikedProducts = current_user.dislikedProducts.sort_by {|a| a.name}
  end
  
  def addDislikedProduct
    product = Product.find_by_name(params[:product][:name])
    if (current_user.dislikedProducts.include? product)
      render :text => "You already added product #{product.name}!", :status => 500
    else
      DislikedProduct.create(:user_id => current_user.id, :product_id => product.id)
      collection = []
      collection << product
      render :partial => "disliked_product", :collection => collection
    end
  end
  
  def removeDislikedProduct
    DislikedProduct.find(:all, :conditions => ["user_id = ? AND product_id = ?", current_user.id, params[:id]])[0].destroy
    render :text => ""
  end
  
end
