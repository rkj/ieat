desc "Starts default server on port 9068"
task :server do
  sh "./script/server -p 9068"
end