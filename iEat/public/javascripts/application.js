// Place your application-specific JavaScript functions and classes here
// This file is automatically included by javascript_include_tag :defaults
function hide_or_show_elements(count, show, hide) {
  count = typeof(count) == 'undefined' ? 5 : count;
  ["#fav_list li", "#container_products li", "#friends_div li", "#popular_tags li"].each(function(name) { 
    $$(name).each(function(val, index) {
      if (index < count) {
        if (!val.visible()) {
          val[show]();
        }
      } else if (val.visible()){
        val[hide]();
      }
    });
  })
}
