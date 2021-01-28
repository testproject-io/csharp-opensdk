# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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
