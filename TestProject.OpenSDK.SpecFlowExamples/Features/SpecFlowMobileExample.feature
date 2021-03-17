Feature: A sample SpecFlow feature using a mobile driver
	In order to see beautiful SpecFlow reports on TestProject Cloud
	As a TestProject user
	I want to run SpecFlow scenarios supported by the SDK

	@mobile
	Scenario: Diana tries to log in to the TestProject demo application on their mobile device
		Given Diana wants to use the TestProject demo application
		When she logs in with username John Smith and password 12345
		Then she gains access to the secure part of the application