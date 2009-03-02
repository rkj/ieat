module UsersHelper
  def sortingTag(displayName, fieldName)
    if (fieldName == @sort)
      reverse = !@reverse
    else
      reverse = false
    end
    link_to displayName, :controller => :users, :action => :favourites, :sort => fieldName, :reverse => reverse
  end
  
  def isFriend?(user)
    current_user.friends.include? user
  end
end