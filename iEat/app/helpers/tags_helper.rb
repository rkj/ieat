module TagsHelper
  def popularTags
    arrayofhashes = ActiveRecord::Base.connection.select_all("SELECT tags.id id
      FROM tags, taggings 
      WHERE tags.id = taggings.tag_id 
      GROUP BY tags.name 
      ORDER BY COUNT(1) DESC 
      LIMIT #{current_user.max_viewed_items}")
    tags = []
    arrayofhashes.each do |item|
      tags << Tag.find_by_id(item["id"])
    end
    return tags
  end
  
  def tag_cloud(tags, classes)
    max, min = 0, 0
    tags.each { |t|
      max = t.count.to_i if t.count.to_i > max
      min = t.count.to_i if t.count.to_i < min
    }

    divisor = ((max - min) / classes.size) + 1

    tags.each { |t|
      yield t.name, classes[(t.count.to_i - min) / divisor]
    }
  end
  
  def recipesTaggedWith(tag)
    recipes = []
    tag.taggings.each do |t|
      recipes << t.taggable
    end
    return recipes
  end
end
