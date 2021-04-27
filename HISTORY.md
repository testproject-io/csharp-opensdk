# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.0] - 2021-04-27

### Added

- Added Control for Reports configuration, it is now possible to state the name and path of the generated report.
- Added Remote Execution Support, it is now possible to execute tests on remote agents in the same network.
- Added the underlying selenium version TestProject SDK uses to the SDK documentaion.
- ([#160](https://github.com/testproject-io/csharp-opensdk/issues/160)) - Added a null check on infered Specflow project name when using the BeforeTestRun annotation.


### Fixed

- ([#165](https://github.com/testproject-io/csharp-opensdk/issues/165)) - Fix for Mobile multitest agent dev session reuse.
- ([#164](https://github.com/testproject-io/csharp-opensdk/issues/164)) - Fix for GenericDriver multitest session reuse.
- ([#162](https://github.com/testproject-io/csharp-opensdk/issues/162)) - Fix for AgentClient creation on Mobile tests, it will now reuse the previously created session in the constructor.
- ([#161](https://github.com/testproject-io/csharp-opensdk/issues/161)) - Fix for Agent Session reuse, tests with the same Project name and Job name will be aggregated in the Test Report.
- ([#159](https://github.com/testproject-io/csharp-opensdk/issues/159)) - Fix for Skipped reports if the Specflow plugin was not installed, will now show a clear error message.

## [1.0.0] - 2021-04-01

### Added

- ([#157](https://github.com/testproject-io/csharp-opensdk/issues/157)) - Allow controlling report type - local / cloud or both.
- ([#158](https://github.com/testproject-io/csharp-opensdk/issues/158)) - Added DriverBuilder.

## [0.66.0] - 2021-03-19

### Added

- Support for Android and iOS mobile drivers
- Examples for mobile drivers (Android and iOS), including SpecFlow-based examples

### Changed

- ([#136](https://github.com/testproject-io/csharp-opensdk/issues/136)) - Open SDK reporting does not show failures
- ([#131](https://github.com/testproject-io/csharp-opensdk/issues/131)) - Additional NulllPointer is thrown when device not found
- ([#116](https://github.com/testproject-io/csharp-opensdk/issues/116)) - Generic driver shouldn't try to take screenshot
- ([#111](https://github.com/testproject-io/csharp-opensdk/issues/111)) - Improve job name inferring logic in SpecFlow plugin to reflect feature name
- ([#105](https://github.com/testproject-io/csharp-opensdk/issues/105)) - Improve exception handling and error message in case no driver is found when using the SpecFlow plugin

### Fixed

- ([#144](https://github.com/testproject-io/csharp-opensdk/issues/144)) - Unable to run same multi browser tests with OpenSDK features
- ([#129](https://github.com/testproject-io/csharp-opensdk/issues/129)) - The IWebDriver object must implement or wrap a driver that implements IHasTouchScreen
- ([#127](https://github.com/testproject-io/csharp-opensdk/issues/127)) - Implicit timeout command is not reported in the proper test
- ([#125](https://github.com/testproject-io/csharp-opensdk/issues/125)) - ResetApp throws System.NullReferenceException
- ([#123](https://github.com/testproject-io/csharp-opensdk/issues/123)) - Failing driver command is not reported when using implicit wait
- ([#117](https://github.com/testproject-io/csharp-opensdk/issues/117)) - [NUnit] elementDictionary (Parameter 'The specified dictionary does not contain an element reference')'
- ([#109](https://github.com/testproject-io/csharp-opensdk/issues/109)) - SDK throws NullReferenceException when agent isn't running
- ([#106](https://github.com/testproject-io/csharp-opensdk/issues/106)) - All Scenarios and Steps are under a single test when using GenericDriver with SpecFlow plugin
- ([#103](https://github.com/testproject-io/csharp-opensdk/issues/103)) - When TP_KEEP_DRIVER_SESSION is set, test names are not reported
- ([#91](https://github.com/testproject-io/csharp-opensdk/issues/91)) - Generated reports on xunit contain wrong test details
- ([#90](https://github.com/testproject-io/csharp-opensdk/issues/90)) - driver.ExecuteScript fails when using driver from OpenSDK
- ([#89](https://github.com/testproject-io/csharp-opensdk/issues/89)) - element.GetAttribute(\*) returns null when using FirefoxDriver
- ([#88](https://github.com/testproject-io/csharp-opensdk/issues/88)) - Test name in report is wrong (after using driver.Quit())
- ([#87](https://github.com/testproject-io/csharp-opensdk/issues/87)) - Updated incorrect code samples in README.md
- ([#85](https://github.com/testproject-io/csharp-opensdk/issues/85)) - Stub OpenSDK C# test fails in ChromeDriver constructor

## [0.65.1] - 2021-01-28

### Added

- Added flag to allow preventing driver from closing its session when process exits.

### Changed

- ([#74](https://github.com/testproject-io/csharp-opensdk/issues/74)) - Updated SpecFlow plugin implementation to be compatible with SpecFlow 3.6

### Fixed

- ([#78](https://github.com/testproject-io/csharp-opensdk/issues/78)) - Improved project and job name inferring for all supported unit testing frameworks
- ([#77](https://github.com/testproject-io/csharp-opensdk/issues/77)) - Test name not always reported when driver.Quit() is not called

## [0.65.0] - 2021-01-25

Initial release.
