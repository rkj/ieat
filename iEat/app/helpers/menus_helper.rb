module MenusHelper
  def isSelectedRecipe(recipe)
    current_user.selectedRecipes.include? recipe
  end
  
  def getAmountsFromFridgeForProduct(name)
    arrayofhashes = ActiveRecord::Base.connection.select_all("SELECT units.name unit_name, SUM(container_products.amount) amount 
      FROM containers, container_products, products, units 
      WHERE containers.owner_id = #{current_user.id} AND 
            containers.id = container_products.container_id AND 
            container_products.product_id = products.id AND
            container_products.unit_id = units.id AND
            products.name = '#{name}' 
      GROUP BY units.name 
      ORDER BY units.name;")
    amounts = []
    arrayofhashes.each do |item|
      amounts << [item["amount"], item["unit_name"]]
    end
    return amounts
  end
end
