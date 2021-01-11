Feature: SpecFlowExample
	In order to see beautiful SpecFlow reports on TestProject Cloud
	As a TestProject user
	I want to run SpecFlow scenarios supported by the SDK

Scenario: Alex tries to log in to the TestProject demo application
    Given Alex wants to use the TestProject demo application
	When he logs in with username John Smith and password 12345
	Then he gains access to the secure part of the application

Scenario: Bernard tries to log in to the TestProject demo application
    Given Bernard wants to use the TestProject demo application
	When he logs in with username John Smith and password 98765
	Then he gains access to the secure part of the application

Scenario: Claire tries to log in to the TestProject demo application
    Given Claire wants to use the TestProject demo application
	When she logs in with username John Smith and password 12345
	Then she gains access to the secure part of the application