#!/usr/bin/ruby
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.

require 'set'
require 'pp'

LEARN_PLURAL = true
RESULTS_COUNT = 1000

def stem(word)
  word
end

class Document
  def initialize(body, keywords, recipe)
    @body = body;
    @keywords = count_keywords(keywords)
    @recipe = recipe
  end

  def count_keywords(keywords)
    kws = {}
    @body.split.each { |words|
      words.each { |niceword| 
        stemword = stem(niceword)
        if keywords.include?(stemword)
          kws[stemword] = 0 unless kws.has_key?(stemword)
          kws[stemword] += 1
        end
      }
    }
    kws
  end
  attr_accessor :keywords, :body
  attr_accessor :weights
  attr_reader :recipe
end

class TfIdfModel
  def initialize(documents, keywords)
    @keywords = keywords
    @documents = documents
    @idfs = count_idf()
    count_weights()
  end

  def noDocsWithKeywords(*keywords)
    @documents.map{ |doc| 
      keywords.all? { |kw| doc.keywords.has_key?(kw) }
    }.inject(0) { |sum, cr| sum + (cr ? 1 : 0) }
  end
  
  def count_idf()
    idfs = {}
    @documentsCountWithKW = {}
    @keywords.each { |kw|
      documentsCount = 0
      @documents.each { |doc|
        if doc.keywords.has_key?(kw)
          documentsCount += 1 
        end
      }
      @documentsCountWithKW[kw] = documentsCount
      if documentsCount == 0
        idfs[kw] = 0
      else
        idfs[kw] = Math.log(1.0*@documents.size/documentsCount)
      end
    }
    # pp idfs
    idfs
  end

  def set_weight(doc)
    w = {}
    doc.keywords.each { |kw, count| w[kw] = count * @idfs[kw]}
    doc.weights = w
  end

  def count_weights
    @documents.each { |doc|
      set_weight(doc)
      # pp doc.weights
    }
  end

  def length(keywords)
    if keywords.is_a?(Document)
      return length(keywords.weights)
    end
    if keywords.is_a?(Hash)
      l = keywords.inject(0.0) { |sum, h| sum + h[1] * h[1]}
      Math.sqrt(l)
    end
  end

  def dot_product(kw1, kw2) 
    sum = 0.0
    kw1.each { |k, v| sum += v*kw2[k]	if kw2.has_key?(k) }
    sum
  end

  def similarity(doc1, doc2)
    dot_product(doc1.weights, doc2.weights) / (length(doc1) * length(doc2))
  end

  def query(query_string)
  	query = Document.new(query_string, @keywords, nil)
  	find_similar(query)
  end
  
  def find_similar(query)
    return [] if query.keywords.empty?
    set_weight(query)
    similarities = {}
    
    @documents.each { |doc|	
      sim = similarity(query, doc)
      similarities[doc] = sim # if sim > 0
    }
    similarities.sort { |a, b|
      if a[1].nan? or b[1].nan?
        pp a, b
      end
      b[1] <=> a[1] 
    }
  end
end

def readData(directory = ".")
  keywords = []
  File.open(directory + '/keywords.txt').each_line { |line|
    keywords << stem(line.chomp)
  }
  keywords.uniq!

  document = ""
  documents = []
  File.open(directory + '/documents.txt').each_line { |line|
    if line =~ /^\s*$/
      documents << Document.new(document, keywords)
      document = ""
    else
      document += line
    end
  }
  return documents, keywords
end

if __FILE__ == $PROGRAM_NAME
  tfi = TfIdfModel.new(*readData("TextCorpora/NewTestCorpus/"))
  pp tfi.correlations("snake".stem)
end
