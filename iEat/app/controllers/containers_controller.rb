class ContainersController < ApplicationController
  layout "standard"
  auto_complete_for :unit, :name
  auto_complete_for :product, :name
  skip_before_filter :verify_authenticity_token, :only => [:auto_complete_for_unit_name, :auto_complete_for_product_name]
  skip_before_filter :login_required#, :only => [:index, :show]
  
  # GET /containers
  # GET /containers.xml
  def index
    if (params[:user])
      @containers = User.find_by_login(params[:user]).containers
    else
      if (current_user.nil?)
        redirect_to :controller => :users, :action => :login
        return
      else
        @containers = current_user.containers
      end
    end

    respond_to do |format|
      format.html # index.html.erb
      format.xml  { render :layout => false }
    end
  end

  # GET /containers/1
  # GET /containers/1.xml
  def show
    @container = Container.find(params[:id]) 
    unless current_user.nil? or @container.owner == current_user
      flash[:notice] = "You can view only you own containers!"
      return redirect_to(:action => "index")
    end

    respond_to do |format|
      format.html # show.html.erb
      format.xml  { render :layout => false }
    end
  end

  # GET /containers/new
  # GET /containers/new.xml
  def new
    @container = Container.new
    
    respond_to do |format|
      format.html { render :action => :edit }
      format.xml  { render :action => :edit, :xml => @container }
    end
  end

  # GET /containers/1/edit
  def edit
    @container = Container.find(params[:id])
  end

  # POST /containers
  # POST /containers.xml
  def create
    @container = current_user.containers.build(params[:container])
    
    respond_to do |format|
      if @container.save
        flash[:notice] = 'Container was successfully created.'
        format.html { redirect_to(@container) }
        format.xml  { render :xml => @container, :status => :created, :location => @container }
      else
        format.html { render :action => "edit" }
        format.xml  { render :xml => @container.errors, :status => :unprocessable_entity }
      end
    end
  end

  # PUT /containers/1
  # PUT /containers/1.xml
  def update
    @container = Container.find(params[:id])

    respond_to do |format|
      if @container.update_attributes(params[:container])
        flash[:notice] = 'Container was successfully updated.'
        format.html { redirect_to(@container) }
        format.xml  { head :ok }
      else
        format.html { render :action => "edit" }
        format.xml  { render :xml => @container.errors, :status => :unprocessable_entity }
      end
    end
  end

  def addContainerProduct   
    @container = Container.find_by_id(params[:id])
    date = Date.parse(params[:product][:date].gsub('/', '-')[0,10])
    @container_product = createContainerProduct(params[:unit], params[:product], params[:amount], date)
    @container.container_products << @container_product
    @container.save!
    @first_container = (current_user.containers[0].id == @container.id)
    unless request.xml_http_request?
      redirect_to :action => :show, :id => @container.id
    end
  rescue => e
    if request.xml_http_request?
      @err = "Unit's " + @container_product.unit.errors.full_messages.join("<br />") + "<br />Product's " + @container_product.product.errors.full_messages.join("<br />") + "<br />" + @container_product.errors.full_messages.join("<br />")
      render :action => "failToAddProduct"
    else
      flash[:notice] = "Unable to add container product."
      redirect_to :action => :show, :id => params[:id]
    end
  end

  def deleteContainerProduct
    cp = ContainerProduct.find_by_id(params[:id])
    container_id = cp.container.id
    cp.destroy
    unless request.xml_http_request?
      redirect_to :action => :show, :id => container_id
    end
  end

  # DELETE /containers/1
  # DELETE /containers/1.xml
  def destroy
    @container = Container.find(params[:id])
    @container.destroy

    respond_to do |format|
      format.html { redirect_to(containers_url) }
      format.xml  { head :ok }
    end
  end
  
  private
    def createContainerProduct(unit, product, amount, expiration_date)
      unit = Unit.find_or_create_by_name(unit[:name])
      product = Product.find_or_create_by_name(product[:name])
      containerProduct = ContainerProduct.new(:amount => amount, :expiration_date => expiration_date)
      containerProduct.product = product
      containerProduct.unit = unit
      return containerProduct
    end  
  
end
