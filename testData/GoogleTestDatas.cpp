#include <gtest/gtest.h>
#include <windows.h>

//Test group displays each test in there various states
TEST(Test,ExpectedFail)
{
	FAIL() << "Expected failure :)";
}
TEST(Test,DISABLED_IgnoredTest)
{
	FAIL() << "This test was run and wasn't supposed to be";
}
TEST(Test,PassedTest)
{
	SUCCEED() << "PASSING";
}

TEST(Test,ExceptionTest)
{
	throw "Exception test";
}

//test group 2 will always pass
TEST(Test2,TestPassing)
{
	SUCCEED() << "PASSING";
}

TEST(UnendingTest,CanAttach)
{
	while (true)
		Sleep(4);
}