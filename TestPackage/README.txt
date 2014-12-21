== About ==
An Test plugin for CPlusPlus projects in VS2008.
Supports Only [http://code.google.com/p/googletest GTest] currently, future plan is to support [http://www.boost.org/doc/libs/1_47_0/libs/test/doc/html/index.html Boost Unit Test] too.
May work with VS2010 but hasn't been tested with it.

== Setup details ==
Requires the Visual Studio SDK to build
7zip is also required to build the vs2010 installtion VIX package

For unit tests change the project setting TestData to your local testdata path.
 
Line to generate reg file:
"RegPkg.exe" /regfile:/TestPackage.reg /codebase "TestPackage.dll" 

For Cplusplus to recognize that a project has tests the first line returned by a call to project.exe --help first line must be: "This program contains tests written using Google Test. You can use the" this line is set in Resources.GTestHelpString. 

Knowing bugs:
GSettings after use defaults is unchecked it can never be checked again, though the defaults are set again when its clicked.
Integration tests do not work this is due to the HostType attribute and vs refusing connections while launching unsure if we should abandon this altogeather or find a fix for this.


== Things Todo ==
* The test marker code. 
* Art work.
* VS2010 testing.
