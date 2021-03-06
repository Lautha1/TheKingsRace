# Remove actions after a certain date (currently December SGX)
cat gourceLog.txt | awk -F\| '$1<=1639785600' > gourceLog.temp
sed -i.bak '/Packages/d' ./gourceLog.temp
sed -i.bak '/ProjectSettings/d' ./gourceLog.temp
sed -i.bak '/Plugins/d' ./gourceLog.temp
sed -i.bak '/Polybrush/d' ./gourceLog.temp
sed -i.bak '/TextMesh/d' ./gourceLog.temp
sed -i.bak '/\.meta/d' ./gourceLog.temp
sed -i.bak '/Addons/d' ./gourceLog.temp
mv gourceLog.temp gourceLog.txt
rm gourceLog.temp.bak

# Setup Project Name
projName="The Kings Race - Unity 3d Project"

function fix {
  sed -i -- "s/$1/$2/g" gourceLog.txt
}

# Replace non human readable names with proper ones
fix "|Seth Berrier|" "|Prof. B|"
fix "|Julia|" "|Julia Butenhoff|"
fix "|Katherine|" "|Katherine Rude|"
fix "|Melbyj1125|" "|Justin Melby|"
fix "|Vladdie|" "|Karl Kazinski|"
fix "|wheatv3015|" "|Vincent Wheat|"
