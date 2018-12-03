$BuildRepo = "BuildSampleRepo"
$SampleRepo = "SampleRepo"

cd c:\tmp\

if(Test-Path $BuildRepo) {rm -r -fo $BuildRepo}
if(Test-Path $SampleRepo) { rm -r -fo $SampleRepo}

git init $BuildRepo
cd $BuildRepo
echo . > 1
git stage --all 
git commit -m 1
git tag 1
mv 1 2
git stage --all
git commit -m 2
git tag -a 2 -m 2
mv 2 3
git stage --all
git commit -m 3
git tag 3
mv 3 4 
git stage --all
git commit -m 4
git checkout -b AltBranch tags/2
mv 2 2.5
git stage --all
git commit -m 2.5
git tag -a 2.5 -m 2.5
mv 2.5 2.6
git stage --all
git commit -m 2.6
git checkout master
cd ..
git clone --bare $BuildRepo $SampleRepo
rm -r -fo $BuildRepo
