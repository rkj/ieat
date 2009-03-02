class MessagesController < ApplicationController  
  def compose
    @user = User.find_by_id(params[:id])
  end
  
  def sendMessage
    addressee = User.find_by_id(params[:user_id])
    addressee.receivedMessages.create(:author_id => current_user.id, :message_type_id => 1, :message => params[:message], 
                                      :read => false, :subject => params[:subject], :date_created => Time.now)
    redirect_to :action => "sent"
  end
  
  def sent
    @sentMessages = UserMessage.find(:all, :conditions => ["author_id = ? AND author_deleted = 'f' AND message_type_id = 1", current_user.id])
  end
  
  def inbox
    @receivedMessages = UserMessage.find(:all, :conditions => ["addressee_id = ? AND addressee_deleted = 'f' AND message_type_id = 1", current_user.id])
  end
  
  def show
    @message = UserMessage.find_by_id(params[:id])
    if (@message.addressee.id == current_user.id && @message.read == false)
      @message.read = true
      @message.save!
    end
  end
  
  def invitations
    @invitations = UserMessage.find(:all, :conditions => ["addressee_id = ? AND addressee_deleted = 'f' AND message_type_id = 2", current_user.id])
  end
    
  def deleteAddresseeMessage
    message = UserMessage.find_by_id(params[:id])
    message.addressee_deleted = true
    message.save!
    render :partial => "menu"
  end
  
  def deleteAuthorMessage
    message = UserMessage.find_by_id(params[:id])
    message.author_deleted = true
    message.save!
    render :partial => "menu"
  end
end
