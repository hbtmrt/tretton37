# Tretton37 downloader
[![](https://tretton37.com//assets/i/contact.jpg)](https://tretton37.com)
The perfect web files downloader.
## Features
- Accepts URI as arguments (If you haven't provided "https://tretton37.com/" will be taken by default.)
- The files are downloaded to a folder called "**Tretton Files**"
- The files are saved aligned with the web file structure.
- The downloading status can be watched as a percentage.
- Once all the files are downloaded, the program shows it as completed in the console.

## Project Structure
1. Tretton37
The console application.
2. Tretton37.Core
A class library project which contains the sharable files across the solution.

## HTML Agility Pack
This third class library was used to download the HTML string and extract the links, scripts, and images.

## Extendability
1. File download strategy
Currently, the program downloads file with asynchronous behaviour. But it can be extended in the future quickly by extending "**IFileDownloader**" interface.

2. Logging
Currently, the program uses a console to write the status. But if a new implementation is needed, that can be quickly done by extending "**ILogHelper**" interface.

3. Resource extraction
Currently the program extracts only images, links and scripts. But that can be extended by extending "**IResourceExtractor**" interface.

## Assumptions
1. This program downloads links, scripts and images **only**.
2. The files are downloaded to the source directory.

## Special Notes
1. A workflow has been added to check the code quality. (GitHub Action)
https://github.com/hbtmrt/tretton37/actions
2. A custom exception class "**ResourceExtractionException**" is used to handle specific errors due to extraction.
3. An asynchronous download mechanism is used to speed the download progress.
