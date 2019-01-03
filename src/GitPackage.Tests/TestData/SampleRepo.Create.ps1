param($SampleRepo)

$BuildRepo = $SampleRepo + "_Build"

if(Test-Path $BuildRepo) {rm -r -fo $BuildRepo}
if(Test-Path $SampleRepo) { rm -r -fo $SampleRepo}

# Branch master has 4 tagged commits.
# Branch AltBranch comes from master tag/2; and has 2 commits.

git init $BuildRepo
pushd $BuildRepo

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

popd
rm -r -fo $BuildRepo
