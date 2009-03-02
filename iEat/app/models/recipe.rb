class Recipe < ActiveRecord::Base
  acts_as_taggable
  
  has_many :recipe_steps, :order => "step_order_no ASC"
  has_many :recipe_comments
  has_many :recipe_ratings
  belongs_to :owner, :class_name => "User", :foreign_key => :owner_id
  belongs_to :editing_user, :class_name => "User", :foreign_key => :editing_user_id
  
  validates_presence_of :name, :owner_id
	validates_numericality_of :number_of_servings, :minutes_to_prepare

  def self.per_page
    15
  end
  
	def ingredients_count
		recipe_steps.inject(0) { |sum,rs| sum + rs.ingredients_count }
	end
	
	def self.search(query, recipes = nil)
	  require 'tf_idf'
	  recipes ||= Recipe.find(:all)
    keywords = Product.find(:all).map { |prod| prod.name.split " " } +
      Recipe.find(:all).map { |recipe| recipe.name.downcase.split(" ") + recipe.tag_list }
	  keywords.flatten!
    # puts keywords
	  documents = recipes.map { |recipe| 
	    doc_txt = ""
	    doc_txt << "#{recipe.name.downcase} " * 3
	    recipe.recipe_steps.each { |rs|
	      rs.step_ingredients.each { |si| doc_txt << si.product.name; doc_txt << " " }
	    }
	    doc_txt << recipe.description
	    Document.new(doc_txt, keywords, recipe)
	  }
	  @tfi = TfIdfModel.new(documents, keywords)
	  @tfi.query(query.downcase). 
	    select{ |doc| doc[1] > 0 }. #documents with some similarity
	    map{ |doc| doc[0].recipe } #recipes
  end
  
  def self.searchByName(query, recipes = nil)
    require 'tf_idf'
    recipes ||= Recipe.find(:all)
    keywords = recipes.map { |recipe| recipe.name.downcase.split " " }
    keywords.flatten!
    documents = recipes.map { |recipe|
      Document.new(recipe.name.downcase, keywords, recipe)
    }
    @tfi = TfIdfModel.new(documents, keywords)
    @tfi.query(query.downcase).
      select { |doc| doc[1] > 0}. #documents with some similarity
      map{ |doc| doc[0].recipe } #recipes
  end
  
  def self.searchByIngredients(query, recipes = nil)
    require 'tf_idf'
    recipes ||= Recipe.find(:all)
    keywords = Product.find(:all).map { |prod| prod.name.split " " }
    keywords.flatten!
    documents = recipes.map { |recipe|
      doc_txt = ""
      recipe.recipe_steps.each { |rs| 
        rs.step_ingredients.each { |si|
          doc_txt << si.product.name
          doc_txt << " "
        }
      }
      Document.new(doc_txt, keywords, recipe)
    }
    @tfi = TfIdfModel.new(documents, keywords)
    @tfi.query(query.downcase).
      select{ |doc| doc[1] > 0 }. #documents with some similarity
      map{ |doc| doc[0].recipe } #recipes
  end
        
  def self.paginatedSearch(query, page, type = "search", user_id = 0, recipes = nil)
    page ||= 1
    WillPaginate::Collection.create(page, Recipe.per_page) do |pager|
      result = Recipe.send type, query, recipes
      
      result.sort!{ |b, a| a.percentFulfilled(user_id) <=> b.percentFulfilled(user_id)} unless user_id == 0
      
      resultsCount = result.length
      result = result[pager.offset, pager.per_page] if result.length > pager.offset
      pager.replace(result)
      pager.total_entries = resultsCount
    end
  end
  
  def self.paginatedAllSorted(page, user_id)
    page ||= 1
    WillPaginate::Collection.create(page, Recipe.per_page) do |pager|
      result = Recipe.find(:all)
      
      result.sort!{ |b, a| a.percentFulfilled(user_id) <=> b.percentFulfilled(user_id)} unless user_id == 0
      
      resultsCount = result.length
      result = result[pager.offset, pager.per_page] if result.length > pager.offset
      pager.replace(result)
      pager.total_entries = resultsCount
    end
  end
  
  def allIngredients
    self.recipe_steps.collect { |rs| rs.step_ingredients }.flatten
  end
  
  def missingProducts(user_id)
    c = Container.find(:first, :conditions => [ "owner_id = #{user_id}" ])
    missing = allIngredients
    missing.reject! { |i| c.products.include?(i.product) } unless c.nil?
    return missing
  end
  
  def missingProductsCount(user_id)
    missingProducts(user_id).size
  end
  
  def percentFulfilled(user_id)
    allCount = allIngredients.size
    (allCount - missingProductsCount(user_id)).to_f/allCount
  end  
  
  def ownerLogin
    self.owner.login
  end
  
  def averageRating
    return 0 if self.recipe_ratings.length == 0
    self.recipe_ratings.inject(0.0) { |sum, el| sum + el.rating } / self.recipe_ratings.length;
  end
  
  def ratingFromUser(user_id)
    rr = RecipeRating.find(:first, :conditions => ["user_id = ? AND recipe_id = ?", user_id, self.id])
    rr.rating if rr
  end
end
