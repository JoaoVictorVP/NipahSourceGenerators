# Nipah Source Generators

### Installation
 Visual studio has a problem with dependencies and source generators, so, in order to use this you would need to take further steps.
 First, download the folder NipahSourceGenerators.Core/NipahSourceGenerators, then, save it anywere in your disk for later uses.
 Second, open your source generator project edit file
 Third, place the following code on it:
 ```csproj
  <Compile Include="C:\Users\Furude Rika\source\repos\NipahSourceGenerators\NipahSourceGenerators.Core\NipahSourceGenerators\**\*.*">
      <Link>%(RecursiveDir)%(FileName)%(Extension)</Link>
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Compile>
  </ItemGroup>

  <ItemGroup>
    <Compile Remove="C:\Users\Furude Rika\source\repos\NipahSourceGenerators\NipahSourceGenerators.Core\NipahSourceGenerators\Thumbs.db" />
  </ItemGroup>
 ```
 Now it should work properly when compilling!

### Usage
  ***WIP***, but very intuitive when you put your hands on it
