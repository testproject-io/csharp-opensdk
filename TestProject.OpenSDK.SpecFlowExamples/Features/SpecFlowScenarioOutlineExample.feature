Feature: SpecFlow example containing a scenario outline
	In order to see beautiful SpecFlow reports on TestProject Cloud
	As a TestProject user
	I want to run SpecFlow scenarios supported by the SDK

	Scenario Outline: User tries to log in to the TestProject demo application
		Given <firstname> wants to use the TestProject demo application
		When he logs in with username <username> and password <password>
		Then he gains access to the secure part of the application
		Examples:
			| firstname | username   | password |
			| Alex      | John Smith | 12345    |
			| Bernard   | John Smith | 98765    |
			| Claire    | John Smith | 12345    |
