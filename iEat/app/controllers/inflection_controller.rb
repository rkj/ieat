class InflectionController < ApplicationController
	# scaffold :ingredient, :suffix => true
	# scaffold :unit, :suffix => true
	in_place_edit_for :unit, :name_genitive
	in_place_edit_for :unit, :name_plural
	in_place_edit_for :product, :name_genitive
	in_place_edit_for :product, :name_plural

	def products
		@products = Product.find(:all, :order => 'name')
	end

	def units
		@units = Unit.find(:all, :order => 'name')
	end

	def destroy_product
		begin
			Product.find_by_id(params[:id]).destroy
			render :text => "Product removed"
		rescue => e
			render :text => "Unable to remove ingredient because: #{e.to_s}"
		end
	end
	
	def destroy_unit
		begin
			Unit.find_by_id(params[:id]).destroy
			render :text => "Unit removed"
		rescue => e
			render :text => "Unable to remove unit because: #{e.to_s}"
		end
	end
end
