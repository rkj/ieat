module Builder
  class XmlMarkup
    def add_record!(record, options = {})
      options = {:builder => self,:skip_instruct => true}.merge!(options)
      ActiveRecord::XmlSerializer.new(record, options).serialize
    end

    def add_record_attributes!(record, options = {})
      record.attribute_names.each do|name|
        attr = ActiveRecord::XmlSerializer::Attribute.new(name,record)
        tag!( attr.name, attr.value.to_s, attr.decorations )
      end
    end
  end
end
