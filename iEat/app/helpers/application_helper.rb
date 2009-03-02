# Methods added to this helper will be available to all templates in the application.
module ApplicationHelper
  def nilIfEmpty(thing)
    return nil if thing.nil? || thing.empty? || (thing =~ /^\s+$/) != nil
    thing
  end

	def pl_pluralize(thing, amount)
		ret = real_pluralize(thing, amount)
		return ret unless nilIfEmpty(ret).nil?
		return thing.name
	end
	
	def real_pluralize(thing, amount)
		if amount - amount.to_i == 0
			if amount == 0
				thing.name
			elsif amount == 1
				thing.name
			elsif (2..4).include?(amount % 10) && (amount < 10 || amount > 20)
				thing.name_plural
			else
				thing.name_genitive
			end
		else
			return thing.name_plural
		end
	end
	
	def format_product(thing)
		"#{format_amount(thing.amount)} #{format_unit(thing.unit, thing.amount)} #{pl_pluralize(thing.product, thing.amount)}"
	end
	
	def format_amount(amount)
		if amount != 0
			if (amount == amount.to_i)
		 		amount.to_i.to_s
			else
				number_with_delimiter(amount, " ", ",")
			end
		else 
			""
		end
	end
	
	def format_unit(unit, amount)
		return pl_pluralize(unit, amount) unless unit.nil? || unit.name == 'brak' || unit.name == '-'
		return ""
	end
	
	def hide_or_show_elements(show="slideDown", hide="slideUp")
	  mvi = current_user ? current_user.max_viewed_items : 0
	  "hide_or_show_elements(#{mvi}, '#{show}', '#{hide}')"
  end
  
  def efect_and_remove(el, effect="slideUp")
    return <<-END
      wasVisible = #{el}.visible()
      if (wasVisible) {
       #{el}.#{effect}({afterFinish: function() {#{el}.remove();} }); 
      } else {
        #{el}.remove();
      }
      #{hide_or_show_elements()}
    END
  end
  
  def show_first_nonvisible(id, effect="slideDown")
    "var done = ! wasVisible;
    $$('#{id}').each(function(value) {
      if (!value.visible() && !done) {
        value.#{effect}();
        done = true;
      }
    });
    "
  end
end
